using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectablePreferenceController : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler
{
    [Header("Components")] [SerializeField]
    private Selectable _mainChild;

    public Selectable MainChild => _mainChild;
    
    [SerializeField] private Image _backgroundImage;

    [Header("Selection properties")] [SerializeField]
    private Color _normalColor;
    [SerializeField] private Color _highlightedColor;
    [SerializeField] private Color _selectedColor;

    public event Action<SelectablePreferenceController> OnControllerSelected;

    private bool m_isHighlighted = false;
    private bool m_isSelected = false;
    private SelectablePreferenceGroup m_parentGroup;
    private int m_indexInGroup;
    private ScrollRect m_parentScrollRect;
    private Selectable[] m_childrenSelectable;

    protected virtual void Start()
    {
        m_isHighlighted = false;
        m_isSelected = false;
        UpdateBackgroundColor();
        m_parentGroup = GetComponentInParent<SelectablePreferenceGroup>();
        m_parentScrollRect = GetComponentInParent<ScrollRect>();
        m_childrenSelectable = GetComponentsInChildren<Selectable>();
    }

    public void Init(SelectablePreferenceGroup group, int index)
    {
        m_parentGroup = group;
        m_indexInGroup = index;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Select();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_isHighlighted = true;
        UpdateBackgroundColor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_isHighlighted = false;
        UpdateBackgroundColor();
    }

    public void OnSelect(BaseEventData eventData)
    {
        Select();
    }

    private void Select()
    {
        if (m_isSelected)
        {
            return;
        }
        
        m_isSelected = true;
        UpdateBackgroundColor();
        OnControllerSelected?.Invoke(this);
        
        // select main child
        if (_mainChild != null)
        {
            _mainChild.Select();
        }
        
        // autoscroll / TODO autoscroll only when outside viewport
        if (m_parentScrollRect != null)
        {
            m_parentScrollRect.verticalNormalizedPosition = 1f - ((float)m_indexInGroup / ( m_parentGroup.ControllerCount - 1));
        }
    }

    public void Deselect()
    {
        m_isSelected = false;
        UpdateBackgroundColor();
    }

    private void UpdateBackgroundColor()
    {
        if (m_isSelected)
        {
            _backgroundImage.color = _selectedColor;
        }
        else if (m_isHighlighted)
        {
            _backgroundImage.color = _highlightedColor;
        }
        else
        {
            _backgroundImage.color = _normalColor;
        }
    }

    public void SetNavigationRight(Selectable selectable)
    {
        foreach (var childSelectable in m_childrenSelectable)
        {
            var childNav = Utils.CloneNavigation(childSelectable.navigation);
            childNav.selectOnRight = selectable;
            childSelectable.navigation = childNav;
        }
    }

    public void SetNavigationLeft(Selectable selectable)
    {
        foreach (var childSelectable in m_childrenSelectable)
        {
            var childNav = Utils.CloneNavigation(childSelectable.navigation);
            childNav.selectOnLeft = selectable;
            childSelectable.navigation = childNav;
        }
    }

    public void SetNavigationUp(Selectable selectable)
    {
        foreach (var childSelectable in m_childrenSelectable)
        {
            var childNav = Utils.CloneNavigation(childSelectable.navigation);
            childNav.selectOnUp = selectable;
            childSelectable.navigation = childNav;
        }
    }

    public void SetNavigationDown(Selectable selectable)
    {
        foreach (var childSelectable in m_childrenSelectable)
        {
            var childNav = Utils.CloneNavigation(childSelectable.navigation);
            childNav.selectOnDown = selectable;
            childSelectable.navigation = childNav;
        }
    }
}