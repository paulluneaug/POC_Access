using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectablePreferenceController : Selectable
    // , ISelectHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Components")] 
    [SerializeField] private Image _backgroundImage;

    [Header("Selection properties")] 
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _highlightedColor;
    [SerializeField] private Color _selectedColor;

    public event Action<SelectablePreferenceController> OnControllerSelected;
    
    private bool _isHighlighted = false;
    private bool _isSelected = false;

    // protected virtual void Start()
    protected override void Start()
    {
        base.Start();
        _isHighlighted = false;
        _isSelected = false;
        UpdateBackgroundColor();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _isSelected = true;
        OnControllerSelected?.Invoke(this);
        _backgroundImage.color = _selectedColor;
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
        _isSelected = true;
        OnControllerSelected?.Invoke(this);
        UpdateBackgroundColor();
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
}