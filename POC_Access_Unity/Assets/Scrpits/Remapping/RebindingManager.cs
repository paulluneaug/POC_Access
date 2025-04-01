using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

using UnityUtility.Singletons;

public class RebindingManager : MonoBehaviourSingleton<RebindingManager>
{
    public bool RebindingOperationRunning => m_rebindingOperationRunning;

    [Header("Inputs References")]
    [SerializeField] private InputActionAsset m_actionAsset;
    [SerializeField] private InputActionReference[] m_remappableActions;

    [SerializeField] private GameObject m_remappableActionControllerPrefab;

    [Header("UI References")]
    [SerializeField] private RectTransform m_controllersParent;
    [SerializeField] private Button m_saveRebindingButton;
    [SerializeField] private Button m_defaultButton;

    [SerializeField] private RebindingInfosController m_rebindingInfosController;

    [NonSerialized] private RebindableActionController[] m_remapableControllers;

    [NonSerialized] private bool m_rebindingOperationRunning;

    private readonly List<RebindableActionController> m_modifiedControllers = new();

    public override void Initialize()
    {
        base.Initialize();
        LoadCanvas();
    }

    private void LoadCanvas()
    {
        LoadRebinding();

        int actionsCount = m_remappableActions.Length;
        m_remapableControllers = new RebindableActionController[actionsCount];

        for (int iAction = 0; iAction < actionsCount; ++iAction)
        {
            InputActionReference remapableAction = m_remappableActions[iAction];
            if (remapableAction.action.bindings[0].isComposite)
            {
                Debug.Log($"Composite, {remapableAction.action.name}");
            }
            RebindableActionController actionController = Instantiate(m_remappableActionControllerPrefab).GetComponent<RebindableActionController>();

            actionController.gameObject.name = $"{remapableAction.name}_Controller";
            actionController.gameObject.SetActive(true);
            actionController.transform.SetParent(m_controllersParent);
            actionController.Init(this, remapableAction);

            actionController.OnRebindingStart += OnRebindingStarts;
            actionController.OnRebindingEnds += OnRebindingEnds;
            actionController.OnRebindingCompleted += OnRebindingCompleted;
            actionController.OnRebindingCancelled += OnRebindingCancelled;

            m_remapableControllers[iAction] = actionController;
        }

        m_rebindingInfosController.HideOperationInfos();

        m_saveRebindingButton.onClick.AddListener(OnSaveButtonClicked);
        m_defaultButton.onClick.AddListener(OnDefaultButtonClicked);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        int actionsCount = m_remappableActions.Length;
        for (int iAction = 0; iAction < actionsCount; ++iAction)
        {
            RebindableActionController actionController = m_remapableControllers[iAction];
            actionController.OnRebindingStart -= OnRebindingStarts;
            actionController.OnRebindingEnds -= OnRebindingEnds;
            actionController.OnRebindingCompleted -= OnRebindingCompleted;
            actionController.OnRebindingCancelled -= OnRebindingCancelled;
            actionController.Dispose();
        }

        m_saveRebindingButton.onClick.RemoveListener(OnSaveButtonClicked);
        m_defaultButton.onClick.RemoveListener(OnDefaultButtonClicked);
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

    private void OnRebindingCompleted(RebindableActionController controller)
    {
        Debug.Log("Added binding");
        m_modifiedControllers.Add(controller);
    }

    private void OnRebindingCancelled(RebindableActionController controller)
    {
        Debug.Log("Rebiding cancelled");
        m_modifiedControllers.Clear();
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
    }

    private void OnDefaultButtonClicked()
    {
        foreach (var controller in m_remapableControllers)
        {
            controller.SetDefaultBinding();
        }
        m_modifiedControllers.Clear();
    }
}
