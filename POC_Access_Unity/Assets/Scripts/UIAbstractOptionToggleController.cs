using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAbstractOptionToggleController : UIAbstractOption<bool>
{
    [Header("Components")] 
    [SerializeField] private Toggle _toggle;

    [SerializeField] private Button _defaultButton;
    [SerializeField] private TMP_Text _valueText;

    [Header("Preferences")] 
    [SerializeField] private string _preferenceName;
    [SerializeField] private bool _defaultValue;


    private void Start()
    {
        _toggle.onValueChanged.AddListener(OnValueChanged);
        var value = IntToBool(PlayerPrefs.GetInt(_preferenceName, BoolToInt(_defaultValue)));
        _toggle.isOn = value;
        _defaultButton.onClick.AddListener(SetDefault);
    }

    private void OnValueChanged(bool value)
    {
        PlayerPrefs.SetInt(_preferenceName, BoolToInt(value));
        _valueText.text = value ? "On" : "Off";
        TriggerValueChanged(value);
    }

    private bool IntToBool(int value)
    {
        return value != 0;
    }

    private int BoolToInt(bool value)
    {
        return value ? 1 : 0;
    }

    public override void SetDefault()
    {
        _toggle.isOn = _defaultValue;
    }
}