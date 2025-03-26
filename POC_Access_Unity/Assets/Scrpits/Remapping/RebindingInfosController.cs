using System;

using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindingInfosController : MonoBehaviour
{
    public class RebindingDisplayInfos
    {
        public string ActionName;
        public string CurrentBindingName;
    }

    [SerializeField] private RectTransform m_infosPanel;
    [SerializeField] private TMP_Text m_mainText;
    [SerializeField] private Button m_cancelOperationButton;

    [NonSerialized] private RebindableActionController m_displayedActionController;
    [NonSerialized] private bool m_operationInfosDisplayed = false;

    private void Awake()
    {
        m_cancelOperationButton.onClick.AddListener(OnCancelButtonClicked);
    }

    private void OnDestroy()
    {
        m_cancelOperationButton.onClick.RemoveListener(OnCancelButtonClicked);
    }

    public void DisplayOperationInfos(RebindableActionController controller)
    {
        m_infosPanel.gameObject.SetActive(true);
        m_operationInfosDisplayed = true;
        m_displayedActionController = controller;

        RebindingDisplayInfos displayInfos = controller.GetDisplayInfos();

        m_mainText.text = $"Rebinding action {displayInfos.ActionName} \nCurrent binding : {displayInfos.CurrentBindingName}";
    }

    public void HideOperationInfos()
    {
        m_infosPanel.gameObject.SetActive(false);
        m_operationInfosDisplayed = false;
        m_displayedActionController = null;
    }

    private void OnCancelButtonClicked()
    {
        if (!m_operationInfosDisplayed)
        {
            return;
        }
        m_displayedActionController.CancelRebindingOperation();
    }
}
