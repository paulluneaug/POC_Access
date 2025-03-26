using System;

using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

using UnityUtility.Extensions;

using static RebindingInfosController;

using RebindingOperation = UnityEngine.InputSystem.InputActionRebindingExtensions.RebindingOperation;

public class RebindableActionController : MonoBehaviour
{
    public event Action<RebindableActionController> OnRebindingStart;
    public event Action<RebindableActionController> OnRebindingEnds;

    [SerializeField] private TMP_Text m_actionNameField;
    [SerializeField] private TMP_Text m_actionBindingField;
    [SerializeField] private Button m_rebindButton;

    [NonSerialized] private RebindingManager m_rebindingManager;

    [NonSerialized] private InputActionReference m_action;
    [NonSerialized] private RebindingOperation m_rebindingOperation;

    [NonSerialized] private DeviceData m_selectedDeviceDatas;

    [NonSerialized] private bool m_wasEnabled = false;

    public void Init(RebindingManager rebindingManager, InputActionReference rebindableAction, DeviceData deviceData)
    {
        m_rebindingManager = rebindingManager;
        m_action = rebindableAction;
        SetDeviceData(deviceData);

        m_actionNameField.text = m_action.action.name;

        m_rebindButton.onClick.AddListener(OnRebindButtonClicked);
    }

    public void SetDeviceData(DeviceData deviceData)
    {
        m_selectedDeviceDatas = deviceData;
        UpdateBindingText();
    }

    public void Dispose()
    {
        m_rebindButton.onClick.RemoveListener(OnRebindButtonClicked);
    }

    public RebindingDisplayInfos GetDisplayInfos()
    {
        return new RebindingDisplayInfos
        {
            ActionName = m_action.action.name,
            CurrentBindingName = m_action.action.GetBindingDisplayString(group: m_selectedDeviceDatas.BindingGroup),
        };
    }

    private void UpdateBindingText()
    {
        m_actionBindingField.text = m_action.action.GetBindingDisplayString(group: m_selectedDeviceDatas.BindingGroup);
    }

    private void OnRebindButtonClicked()
    {
        if (m_rebindingManager.RebindingOperationRunning)
        {
            return;
        }
        StartRebinding();
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
            .WithBindingGroup(m_selectedDeviceDatas.BindingGroup);

        m_selectedDeviceDatas.ControlPaths.ForEach(path => m_rebindingOperation.WithControlsHavingToMatchPath(path));

        if (m_action.action.GetBindingIndex(group: m_selectedDeviceDatas.BindingGroup) == -1)
        {
            _ = m_rebindingOperation.WithRebindAddingNewBinding(m_selectedDeviceDatas.BindingGroup);
        }
    
        _ = m_rebindingOperation.Start();

        Debug.Log("Start rebinding");
        Debug.Log($"{m_action.action.name} - Bindings : ");
        m_action.action.bindings.ForEach(binding => Debug.Log(binding.effectivePath));
    }

    private void OnRebindingCanceled(RebindingOperation rebindingOperation)
    {
        EndRebindingOperation();
    }

    private void OnRebindingComplete(RebindingOperation rebindingOperation)
    {
        EndRebindingOperation();
        UpdateBindingText();

        Debug.Log("End rebinding");
    }

    private void EndRebindingOperation()
    {
        m_action.action.SetEnabled(m_wasEnabled);

        OnRebindingEnds?.Invoke(this);
        m_rebindingOperation = null;
    }
}
