using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindingManager : MonoBehaviour
{
    private const string SAVED_REBINGDING_PATH = "SavedRebinding";

    [Header("Inputs References")]
    [SerializeField] private InputActionAsset m_actionAsset;
    [SerializeField] private InputActionReference[] m_remappableActions;

    [SerializeField] private GameObject m_remappableActionControllerPrefab;

    [Header("UI References")]
    [SerializeField] private RectTransform m_controllersParent;


    [NonSerialized] private RebindableActionController[] m_remapableControllers;

    private void Start()
    {
        int actionsCount = m_remappableActions.Length;
        m_remapableControllers = new RebindableActionController[actionsCount];

        for (int iAction = 0; iAction < actionsCount; ++iAction)
        {
            InputActionReference remapableAction = m_remappableActions[iAction];
            RebindableActionController actionController = Instantiate(m_remappableActionControllerPrefab).GetComponent<RebindableActionController>();

            actionController.gameObject.name = $"{remapableAction.name}_Controller";
            actionController.gameObject.SetActive(true);
            actionController.transform.parent = m_controllersParent;
            actionController.Init(remapableAction);
        }
    }

    public void SaveRebinding()
    {
        string bindingOverride = m_actionAsset.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(SAVED_REBINGDING_PATH, bindingOverride);
    }

    public void LoadRebinding()
    {
        if (PlayerPrefs.HasKey(SAVED_REBINGDING_PATH))
        {
            string savedRebnding = PlayerPrefs.GetString(SAVED_REBINGDING_PATH);
            m_actionAsset.LoadBindingOverridesFromJson(savedRebnding);
        }
    }
}
