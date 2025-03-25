using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectablePreferenceGroup : MonoBehaviour
{
    private SelectablePreferenceController[] m_selectableControllerList;

    private SelectablePreferenceController m_currentSelectedController;
    
    private void Start()
    {
        m_selectableControllerList = GetComponentsInChildren<SelectablePreferenceController>();
        foreach (var controller in m_selectableControllerList)
        {
            controller.OnControllerSelected += OnControllerSelected;
        }
    }

    private void OnControllerSelected(SelectablePreferenceController controller)
    {
        m_currentSelectedController?.OnDeselect();
        m_currentSelectedController = controller;
    }
}
