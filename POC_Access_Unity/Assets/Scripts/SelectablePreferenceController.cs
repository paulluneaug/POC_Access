using System;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectablePreferenceController : Selectable
// , ISelectHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Components")] [SerializeField]
    private Selectable _mainChild;
    [SerializeField] private Image _backgroundImage;

    [Header("Selection properties")] [SerializeField]
    private Color _normalColor;

    [SerializeField] private Color _highlightedColor;
    [SerializeField] private Color _selectedColor;

    public event Action<SelectablePreferenceController> OnControllerSelected;

    private bool _isHighlighted = false;
    private bool _isSelected = false;
    private SelectablePreferenceGroup _parentGroup;
    private int _indexInGroup;
    private ScrollRect _parentScrollRect;

    // protected virtual void Start()
    protected override void Start()
    {
        base.Start();
        _isHighlighted = false;
        _isSelected = false;
        UpdateBackgroundColor();
        _parentGroup = GetComponentInParent<SelectablePreferenceGroup>();
        _parentScrollRect = GetComponentInParent<ScrollRect>();
    }

    public void Init(SelectablePreferenceGroup group, int index)
    {
        _parentGroup = group;
        _indexInGroup = index;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSelect();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        _isHighlighted = true;
        UpdateBackgroundColor();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        _isHighlighted = false;
        UpdateBackgroundColor();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        OnSelect();
    }

    private void OnSelect()
    {
        _isSelected = true;
        OnControllerSelected?.Invoke(this);
        UpdateBackgroundColor();
        if (_parentScrollRect != null)
        {
            if (_indexInGroup == _parentGroup.ControllerCount - 1)
            {
                _parentScrollRect.verticalScrollbar.value = 0;
            }
            else
            {
                _parentScrollRect.verticalScrollbar.value = Mathf.InverseLerp(_parentGroup.ControllerCount, 0, _indexInGroup);
            }
        }
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
    }

    public void OnDeselect()
    {
        _isSelected = false;
        UpdateBackgroundColor();
    }

    private void UpdateBackgroundColor()
    {
        if (_isSelected)
        {
            _backgroundImage.color = _selectedColor;
        }
        else if (_isHighlighted)
        {
            _backgroundImage.color = _highlightedColor;
        }
        else
        {
            _backgroundImage.color = _normalColor;
        }
    }

    public void SetPrevious(SelectablePreferenceController selectableController)
    {
        var nav = CloneNavigation(navigation);
        nav.selectOnUp = selectableController;
        navigation = nav;
        foreach (var childSelectable in GetComponentsInChildren<Selectable>())
        {
            var childNav = CloneNavigation(childSelectable.navigation);
            childNav.selectOnUp = selectableController;
            childSelectable.navigation = childNav;
        }
    }

    public void SetNext(SelectablePreferenceController selectableController)
    {
        var nav = CloneNavigation(navigation);
        nav.selectOnDown = selectableController;
        navigation = nav;
        foreach (var childSelectable in GetComponentsInChildren<Selectable>())
        {
            var childNav = CloneNavigation(childSelectable.navigation);
            childNav.selectOnDown = selectableController;
            childSelectable.navigation = childNav;
        }
    }

    private Navigation CloneNavigation(Navigation navigation)
    {
        return new Navigation
        {
            selectOnUp = navigation.selectOnUp, 
            selectOnDown = navigation.selectOnDown, 
            selectOnLeft = navigation.selectOnLeft, 
            selectOnRight = navigation.selectOnRight,
            mode = navigation.mode,
            wrapAround = navigation.wrapAround
        };
    }
}