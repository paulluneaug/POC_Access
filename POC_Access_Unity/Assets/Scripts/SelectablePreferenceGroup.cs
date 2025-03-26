using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectablePreferenceGroup : MonoBehaviour
{
    private SelectablePreferenceController[] m_selectableControllerList;

    public int ControllerCount => m_selectableControllerList.Length;

    private SelectablePreferenceController m_currentSelectedController;
    
    private void Start()
    {
        m_selectableControllerList = GetComponentsInChildren<SelectablePreferenceController>();
        for (var i = 0; i < m_selectableControllerList.Length; i++)
        {
            var controller = m_selectableControllerList[i];
            controller.Init(this, i);
            controller.OnControllerSelected += OnControllerSelected;
            var previous = m_selectableControllerList[MathUtils.Mod(i - 1, m_selectableControllerList.Length)];
            var next = m_selectableControllerList[MathUtils.Mod(i + 1, m_selectableControllerList.Length)];
            controller.SetPrevious(previous);
            controller.SetNext(next);
        }
    }

    private void OnControllerSelected(SelectablePreferenceController controller)
    {
        m_currentSelectedController?.OnDeselect();
        m_currentSelectedController = controller;
    }
}
