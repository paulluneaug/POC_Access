using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using RebindingOperation = UnityEngine.InputSystem.InputActionRebindingExtensions.RebindingOperation;

public class RebindableActionController : MonoBehaviour
{
    public event Action OnRebindingStart;
    public event Action OnRebindingEnds;

    [SerializeField] private TMP_Text m_actionNameField;
    [SerializeField] private TMP_Text m_actionBindingField;
    [SerializeField] private Button m_rebindButton;

    [NonSerialized] private InputActionReference m_action;
    [NonSerialized] private RebindingOperation m_rebindingOperation;

    public void Init(InputActionReference rebindableAction)
    {
        m_action = rebindableAction;
        m_actionNameField.text = m_action.action.name;

        m_rebindButton.onClick.AddListener(OnRebindButtonClicked);
    }

    public void Dispose()
    {
        m_rebindButton.onClick.RemoveListener(OnRebindButtonClicked);
    }

    private void OnRebindButtonClicked()
    {
        StartRebinding();
    }

    public void CancelRebinding()
    {
        m_rebindingOperation?.Cancel();
    }

    private void StartRebinding()
    {
        OnRebindingStart?.Invoke();

        m_rebindingOperation = m_action.action
            .PerformInteractiveRebinding()
            .OnCancel(OnRebindingCanceled)
            .OnComplete(OnRebindingComplete)
            .Start();
    }

    private void OnRebindingCanceled(RebindingOperation rebindingOperation)
    {
        OnRebindingEnds?.Invoke();
    }

    private void OnRebindingComplete(RebindingOperation rebindingOperation)
    {
        OnRebindingEnds?.Invoke();
        m_rebindingOperation = null;
    }
}
