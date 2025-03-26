using System;

using UnityEngine;
using UnityEngine.InputSystem;

using UnityUtility.Extensions;

using static RebindingInfosController;

public class RebindableActionController : RebindableController
{
    [NonSerialized] protected InputAction m_action;

    [NonSerialized] private bool m_wasActionEnabled = false;

    public virtual void Init(RebindingManager rebindingManager, InputAction rebindableAction, DeviceData deviceData)
    {
        m_action = rebindableAction;
        InitRebindableController(rebindingManager, deviceData);
    }

    //protected virtual void UpdateBindingText_0()
    //{
    //    InputBinding bindingMask = InputBinding.MaskByGroup(m_selectedDeviceDatas.BindingGroup);
    //    InputBinding binding = m_action.action.bindings[m_action.action.GetBindingIndex(bindingMask)];

    //}

    protected override void StartRebindingOperation()
    {
        base.StartRebindingOperation();

        m_wasActionEnabled = m_action.enabled;
        m_action.Disable();

        m_rebindingOperation = m_action
            .PerformInteractiveRebinding()
            .OnCancel(OnRebindingCanceled)
            .OnComplete(OnRebindingComplete)
            .WithBindingGroup(m_selectedDeviceDatas.BindingGroup);

        m_selectedDeviceDatas.ControlPaths.ForEach(path => m_rebindingOperation.WithControlsHavingToMatchPath(path));

        if (m_action.GetBindingIndex(group: m_selectedDeviceDatas.BindingGroup) == -1)
        {
            _ = m_rebindingOperation.WithRebindAddingNewBinding(m_selectedDeviceDatas.BindingGroup);
        }

        _ = m_rebindingOperation.Start();
    }

    public override RebindingDisplayInfos GetDisplayInfos()
    {
        return new RebindingDisplayInfos
        {
            ActionName = m_action.name,
            CurrentBindingName = m_action.GetBindingDisplayString(group: m_selectedDeviceDatas.BindingGroup),
        };
    }

    protected override void UpdateBindingText()
    {
        m_rebindableBindingField.text = m_action.GetBindingDisplayString(group: m_selectedDeviceDatas.BindingGroup);
    }

    protected override string GetRebindableName()
    {
        return m_action.name;
    }
}
