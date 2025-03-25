using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TogglePreferenceController : SelectablePreferenceController
{
    [Header("Components")] 
    [SerializeField] private Toggle _toggle;

    [SerializeField] private Button _resetButton;
    [SerializeField] private TMP_Text _valueText;

    [Header("Preferences")] 
    [SerializeField] private string _preferenceName;
    [SerializeField] private bool _defaultValue;


    protected override void Start()
    {
        base.Start();
        _toggle.onValueChanged.AddListener(OnValueChanged);
        var value = IntToBool(PlayerPrefs.GetInt(_preferenceName, BoolToInt(_defaultValue)));
        _toggle.SetIsOnWithoutNotify(value);
        _valueText.text = value ? "On" : "Off";
        _resetButton.onClick.AddListener(OnReset);
    }

    private void OnValueChanged(bool value)
    {
        PlayerPrefs.SetInt(_preferenceName, BoolToInt(value));
        _valueText.text = value ? "On" : "Off";
    }

    private void OnReset()
    {
        _toggle.isOn = _defaultValue;
    }

    private bool IntToBool(int value)
    {
        return value != 0;
    }

    private int BoolToInt(bool value)
    {
        return value ? 1 : 0;
    }
}