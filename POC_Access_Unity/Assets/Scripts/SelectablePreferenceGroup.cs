using UnityEngine;
using UnityEngine.UI;

public class SelectablePreferenceGroup : MonoBehaviour
{
    [SerializeField] private Selectable m_saveButton;
    [SerializeField] private Selectable m_resetButton;
    [SerializeField] private Selectable m_backButton;
    
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
            controller.SetNavigationUp(previous.MainChild);
            controller.SetNavigationDown(next.MainChild);
        }

        var firstController = m_selectableControllerList[0];
        var lastController = m_selectableControllerList[^1];
        
        firstController.SetNavigationUp(m_saveButton);
        lastController.SetNavigationDown(m_saveButton);
        
        var navigation = Utils.CloneNavigation(m_saveButton.navigation);
        navigation.selectOnUp = lastController.MainChild;
        navigation.selectOnDown = firstController.MainChild;
        m_saveButton.navigation = navigation;
                
        navigation = Utils.CloneNavigation(m_resetButton.navigation);
        navigation.selectOnUp = lastController.MainChild;
        navigation.selectOnDown = firstController.MainChild;
        m_resetButton.navigation = navigation;
                
        navigation = Utils.CloneNavigation(m_backButton.navigation);
        navigation.selectOnUp = lastController.MainChild;
        navigation.selectOnDown = firstController.MainChild;
        m_backButton.navigation = navigation;
    }
    
    private void OnControllerSelected(SelectablePreferenceController controller)
    {
        m_currentSelectedController?.Deselect();
        m_currentSelectedController = controller;
    }

    public void OnCancel()
    {
        m_currentSelectedController?.Deselect();
        m_currentSelectedController = null;
        m_saveButton.Select();
    }
}
