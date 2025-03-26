using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

using UnityUtility.Extensions;

using static UnityEngine.InputSystem.InputActionSetupExtensions;

public class RebindableAxisController : RebindableActionController
{
    [SerializeField] private GameObject m_rebindabeCompositeControllerPrefab;

    [SerializeField] private Toggle m_compositeToggle;
    [SerializeField] private RectTransform m_compositeBindingControllersContainer;

    [NonSerialized] private int m_currentGroupBindingIndex;

    [NonSerialized] private List<RebindableCompositeController> m_compositeControllers;

    public override void Init(RebindingManager rebindingManager, InputAction rebindableAction, DeviceData deviceData)
    {
        base.Init(rebindingManager, rebindableAction, deviceData);
        UpdateCompositeToggle();

        m_compositeControllers = new List<RebindableCompositeController>();
        UpdateCompositeControllers();
    }

    private void UpdateCompositeToggle()
    {
        bool isComposite = false;
        if (m_action.TryGetBindingForGroup(m_selectedDeviceDatas.BindingGroup, out m_currentGroupBindingIndex, out InputBinding binding))
        {
            if (binding.isPartOfComposite)
            {
                binding = m_action.bindings[--m_currentGroupBindingIndex];
            }

            isComposite = binding.isComposite;
        }
        m_compositeToggle.onValueChanged.AddListener(OnCompositeToggleValueChanged);
        m_compositeToggle.SetIsOnWithoutNotify(isComposite);
    }

    public override void SetDeviceData(DeviceData deviceData)
    {
        base.SetDeviceData(deviceData);
        UpdateCompositeToggle();
    }

    public override void Dispose()
    {
        base.Dispose();
        m_compositeToggle.onValueChanged.RemoveListener(OnCompositeToggleValueChanged);
    }

    private void UpdateCompositeControllers()
    {
        m_compositeControllers.ForEach(controller => controller.gameObject.Destroy());
        m_compositeControllers.Clear();

        InputBinding currentGroupBinding = m_action.bindings[m_currentGroupBindingIndex];
        if (!currentGroupBinding.isComposite)
        {
            return;
        }

        BindingSyntax bindingSyntax = m_action.ChangeCompositeBinding(GetCompositeName());

        int iterations = 0;
        int maxIterations = 10;

        BindingSyntax compositePart = bindingSyntax.NextCompositeBinding();
        while (compositePart.valid && iterations++ < maxIterations)
        {
            RebindableCompositeController compositeController = Instantiate(m_rebindabeCompositeControllerPrefab)
                .GetComponent<RebindableCompositeController>();

            compositeController.Init(m_rebindingManager, m_action, compositePart, m_selectedDeviceDatas);
            compositeController.transform.SetParent(m_compositeBindingControllersContainer);

            m_compositeControllers.Add(compositeController);

            compositePart = compositePart.NextCompositeBinding();
        }
    }

    private void OnCompositeToggleValueChanged(bool isComposite)
    {
        SetCompositeUIActive(isComposite);

        if (!isComposite)
        {
            BindingSyntax compositeBinding = m_action.ChangeCompositeBinding(m_action.bindings[m_currentGroupBindingIndex].name);
            compositeBinding.Erase();
        }
        else
        {
            _ = m_action.AddCompositeBinding(GetCompositeName());
        }

        UpdateCompositeControllers();
    }

    private string GetCompositeName()
    {
        return m_action.expectedControlType switch
        {
            "Vector2" => "2DVector",
            "Vector3" => "3DVector",
            _ => throw new NotImplementedException($"The control type {m_action.expectedControlType} is not supported yet")
        };
    }

    private void SetCompositeUIActive(bool isComposite)
    {
        m_rebindButton.gameObject.SetActive(!isComposite);
        m_compositeBindingControllersContainer.gameObject.SetActive(isComposite);
    }
}
