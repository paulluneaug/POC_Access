using System;

using Unity.VisualScripting;

using UnityEngine.InputSystem;

using UnityUtility.Extensions;

using static RebindingInfosController;
using static UnityEngine.InputSystem.InputActionSetupExtensions;

public class RebindableCompositeController : RebindableController
{
    [NonSerialized] private BindingSyntax m_compositeBinding;
    [NonSerialized] private InputAction m_action;


    public virtual void Init(RebindingManager rebindingManager, InputAction rebindableAction, BindingSyntax rebindableComposite, DeviceData deviceData)
    {
        m_action = rebindableAction;
        m_compositeBinding = rebindableComposite;
        InitRebindableController(rebindingManager, deviceData);
    }

    public override RebindingDisplayInfos GetDisplayInfos()
    {
        return new RebindingDisplayInfos
        {
            ActionName = m_compositeBinding.binding.GetNameOfComposite(),
            CurrentBindingName = m_compositeBinding.binding.ToDisplayString(),
        };
    }

    protected override string GetRebindableName()
    {
        return m_compositeBinding.binding.GetNameOfComposite();
    }

    protected override void UpdateBindingText()
    {
        m_rebindableBindingField.text = m_compositeBinding.binding.ToDisplayString();
    }

    protected override void StartRebindingOperation()
    {
        base.StartRebindingOperation();

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
}
