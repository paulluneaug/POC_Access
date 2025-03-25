using System;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

using UnityUtility.Extensions;
using UnityUtility.SerializedDictionary;

public class RebindingManager : MonoBehaviour
{
    public bool RebindingOperationRunning => m_rebindingOperationRunning;

    [Header("Inputs References")]
    [SerializeField] private InputActionAsset m_actionAsset;
    [SerializeField] private InputActionReference[] m_remappableActions;

    [SerializeField] private GameObject m_remappableActionControllerPrefab;

    [Header("UI References")]
    [SerializeField] private RectTransform m_controllersParent;
    [SerializeField] private TMP_Dropdown m_selectedDeviceDropdown;
    [SerializeField] private SerializedDictionary<TMP_Dropdown.OptionData, DeviceData> m_deviceOptions;

    [SerializeField] private Button m_saveRebindingButton;
    [SerializeField] private Button m_quitRebindingButton;

    [SerializeField] private RebindingInfosController m_rebindingInfosController;


    [NonSerialized] private RebindableActionController[] m_remapableControllers;

    [NonSerialized] private bool m_rebindingOperationRunning;


    private void Start()
    {
        LoadRebinding();

        m_selectedDeviceDropdown.options = m_deviceOptions.Keys.ToList();
        m_selectedDeviceDropdown.onValueChanged.AddListener(OnSelectedDeviceChanged);
        m_selectedDeviceDropdown.SetValueWithoutNotify(0);

        TMP_Dropdown.OptionData selectedOption = m_selectedDeviceDropdown.options[0];
        DeviceData selectedDeviceData = m_deviceOptions[selectedOption];


        int actionsCount = m_remappableActions.Length;
        m_remapableControllers = new RebindableActionController[actionsCount];

        for (int iAction = 0; iAction < actionsCount; ++iAction)
        {
            InputActionReference remapableAction = m_remappableActions[iAction];
            RebindableActionController actionController = Instantiate(m_remappableActionControllerPrefab).GetComponent<RebindableActionController>();

            actionController.gameObject.name = $"{remapableAction.name}_Controller";
            actionController.gameObject.SetActive(true);
            actionController.transform.SetParent(m_controllersParent);
            actionController.Init(this, remapableAction, selectedDeviceData);

            actionController.OnRebindingStart += OnRebindingStarts;
            actionController.OnRebindingEnds += OnRebindingEnds;

            m_remapableControllers[iAction] = actionController;
        }

        m_rebindingInfosController.HideOperationInfos();

        m_saveRebindingButton.onClick.AddListener(OnSaveButtonClicked);
        m_quitRebindingButton.onClick.AddListener(Quit);
    }

    private void OnDestroy()
    {
        int actionsCount = m_remappableActions.Length;
        for (int iAction = 0; iAction < actionsCount; ++iAction)
        {
            RebindableActionController actionController = m_remapableControllers[iAction];
            actionController.OnRebindingStart -= OnRebindingStarts;
            actionController.OnRebindingEnds -= OnRebindingEnds;
            actionController.Dispose();
        }

        m_saveRebindingButton.onClick.RemoveListener(OnSaveButtonClicked);
        m_quitRebindingButton.onClick.RemoveListener(Quit);
    }

    private void OnRebindingStarts(RebindableActionController controller)
    {
        m_rebindingOperationRunning = true;
        m_rebindingInfosController.DisplayOperationInfos(controller);
    }

    private void OnRebindingEnds(RebindableActionController controller)
    {
        m_rebindingOperationRunning = false;
        m_rebindingInfosController.HideOperationInfos();
    }

    private void OnSelectedDeviceChanged(int deviceIndex)
    {
        TMP_Dropdown.OptionData selectedOption = m_selectedDeviceDropdown.options[deviceIndex];
        DeviceData selectedDeviceData = m_deviceOptions[selectedOption];
        m_remapableControllers.ForEach(controller => controller.SetDeviceData(selectedDeviceData));
    }

    public void SaveRebinding()
    {
        string bindingOverride = m_actionAsset.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(PlayerPrefsGlossary.SAVED_REBINGDING_PATH, bindingOverride);
    }

    public void LoadRebinding()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsGlossary.SAVED_REBINGDING_PATH))
        {
            string savedRebnding = PlayerPrefs.GetString(PlayerPrefsGlossary.SAVED_REBINGDING_PATH);
            m_actionAsset.LoadBindingOverridesFromJson(savedRebnding);
        }
    }

    private void OnSaveButtonClicked()
    {
        SaveRebinding();
        Quit();
    }

    private void Quit()
    {
        
    }
}
