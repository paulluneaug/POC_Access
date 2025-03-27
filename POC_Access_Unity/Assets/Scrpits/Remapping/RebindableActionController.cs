using System;

using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

using UnityUtility.Extensions;

using static RebindingInfosController;

using RebindingOperation = UnityEngine.InputSystem.InputActionRebindingExtensions.RebindingOperation;

public class RebindableActionController : SelectablePreferenceController
{
    public event Action<RebindableActionController> OnRebindingStart;
    public event Action<RebindableActionController> OnRebindingEnds;
    public event Action<RebindableActionController> OnRebindingCompleted;
    public event Action<RebindableActionController> OnRebindingCancelled;

    [Header("Parameters")] 
    [SerializeField] private string m_controlScheme = "Scheme";
    [SerializeField] private TMP_Text m_actionNameField;
    [SerializeField] private TMP_Text m_actionBindingField;
    [SerializeField] private Button m_rebindButton;
    [SerializeField] private Button m_defaultButton;

    [NonSerialized] private RebindingManager m_rebindingManager;

    [NonSerialized] private InputActionReference m_action;
    [NonSerialized] private RebindingOperation m_rebindingOperation;

    [NonSerialized] private bool m_wasEnabled = false;

    public void Init(RebindingManager rebindingManager, InputActionReference rebindableAction)
    {
        m_rebindingManager = rebindingManager;
        m_action = rebindableAction;
        UpdateBindingText();

        m_actionNameField.text = $"{m_action.action.name} ({m_action.action.actionMap.name})";

        m_rebindButton.onClick.AddListener(OnRebindButtonClicked);
        m_defaultButton.onClick.AddListener(OnDefaultButtonClicked);
    }

    public void Dispose()
    {
        m_rebindButton.onClick.RemoveListener(OnRebindButtonClicked);
        m_defaultButton.onClick.RemoveListener(OnDefaultButtonClicked);
    }

    public RebindingDisplayInfos GetDisplayInfos()
    {
        return new RebindingDisplayInfos
        {
            ActionName = m_action.action.name,
            CurrentBindingName = m_action.action.bindings[0].effectivePath
        };
    }

    private void UpdateBindingText()
    {
        m_actionBindingField.text = m_action.action.bindings[0].effectivePath;
    }

    private void OnRebindButtonClicked()
    {
        if (m_rebindingManager.RebindingOperationRunning)
        {
            return;
        }
        StartRebinding();
    }
    

    private void OnDefaultButtonClicked()
    {
        SetDefaultBinding();
    }

    public void SetDefaultBinding()
    {
        m_action.action.RemoveBindingOverride(0);
        UpdateBindingText();
    }

    public void CancelRebindingOperation()
    {
        m_rebindingOperation?.Cancel();
    }

    private void StartRebinding()
    {
        m_wasEnabled = m_action.action.enabled;
        m_action.action.Disable();

        OnRebindingStart?.Invoke(this);

        m_rebindingOperation = m_action.action
            .PerformInteractiveRebinding()
            .OnCancel(OnRebindingCanceled)
            .OnComplete(OnRebindingComplete)
            .WithBindingGroup(m_controlScheme);

        if (m_action.action.GetBindingIndex(group: m_controlScheme) == -1)
        {
            _ = m_rebindingOperation.WithRebindAddingNewBinding(m_controlScheme);
        }
    
        _ = m_rebindingOperation.Start();

        Debug.Log("Start rebinding");
        Debug.Log($"{m_action.action.name} - Bindings : ");
        m_action.action.bindings.ForEach(binding => Debug.Log(binding.effectivePath));
    }

    private void OnRebindingCanceled(RebindingOperation rebindingOperation)
    {
        EndRebindingOperation();
        OnRebindingCancelled?.Invoke(this);
    }

    private void OnRebindingComplete(RebindingOperation rebindingOperation)
    {
        EndRebindingOperation();
        UpdateBindingText();

        OnRebindingCompleted?.Invoke(this);
    }

    private void EndRebindingOperation()
    {
        m_action.action.SetEnabled(m_wasEnabled);

        OnRebindingEnds?.Invoke(this);
        m_rebindingOperation = null;

        Debug.Log("End rebinding");
    }
}
