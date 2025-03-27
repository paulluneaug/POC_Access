using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIOptionSliderController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Slider _slider;
    [SerializeField] private Button _defaultButton;
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private TMP_Text _valueText;

    [Header("Parameters")] 
    [SerializeField] private float _minValue = 0;
    [SerializeField] private float _maxValue = 1;
    [SerializeField] private float _increment = 0.1f;
    [SerializeField] private bool _wholeNumbers = false;
    [SerializeField] private float _intIncrement = 1f;
    
    [Header("Preferences")]
    [SerializeField] private string _preferenceName;
    [SerializeField] private float _defaultValue;

    private void Start()
    {
        _slider.minValue = _minValue;
        _slider.maxValue = _maxValue;
        _slider.wholeNumbers = _wholeNumbers;
        
        _slider.onValueChanged.AddListener(OnValueChanged);
        var value = PlayerPrefs.GetFloat(_preferenceName, _defaultValue);
        _slider.SetValueWithoutNotify(value);
        _valueText.text = value.ToString(_wholeNumbers ? "N0" : "N1", CultureInfo.InvariantCulture);
        _defaultButton.onClick.AddListener(OnReset);
        _leftButton.onClick.AddListener(OnLeft);
        _rightButton.onClick.AddListener(OnRight);
    }

    private void OnValueChanged(float value)
    {
        PlayerPrefs.SetFloat(_preferenceName, value);
        _valueText.text = value.ToString(_wholeNumbers ? "N0" : "N1", CultureInfo.InvariantCulture);
    }

    private void OnReset()
    {
        _slider.value = _defaultValue;
    }

    private void OnRight()
    {
        _slider.value += _wholeNumbers ? _intIncrement : _increment;
    }

    private void OnLeft()
    {
        _slider.value -= _wholeNumbers ? _intIncrement : _increment;
    }
}
