using UnityEngine;
using UnityEngine.UI;

public class UIAbstractOptionColorController : UIAbstractOption<Color>
{
    [Header("Components")] 
    [SerializeField] private Slider _colorSlider;

    [SerializeField] private RawImage _colorBackground;
    [SerializeField] private Button _defaultButton;
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private Image _valueImage;

    [Header("Parameters")] 
    [SerializeField] private float _increment = 0.1f;

    [Header("Preferences")] 
    [SerializeField] private string _preferenceName;

    [SerializeField] private float _defaultValue;

    private void Start()
    {
        var hueTex = new Texture2D(4, 1);
        hueTex.SetPixels(new Color[] { Color.red, Color.green, Color.blue, Color.red });
        hueTex.Apply();
        _colorBackground.texture = hueTex;
        _colorSlider.onValueChanged.AddListener(OnSliderValueChanged);
        _defaultButton.onClick.AddListener(SetDefault);
        _leftButton.onClick.AddListener(OnLeft);
        _rightButton.onClick.AddListener(OnRight);
        var value = PlayerPrefs.GetFloat(_preferenceName, _defaultValue);
        _colorSlider.value = value;
    }

    private void OnSliderValueChanged(float value)
    {
        var color = Color.HSVToRGB(value, 1, 1);
        _valueImage.color = color;
        PlayerPrefs.SetFloat(_preferenceName, value);
        TriggerValueChanged(color);
    }

    private void OnRight()
    {
        _colorSlider.value += _increment;
    }

    private void OnLeft()
    {
        _colorSlider.value -= _increment;
    }

    public override void SetDefault()
    {        
        _colorSlider.value = _defaultValue;
    }
}