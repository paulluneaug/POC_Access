using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityUtility.Extensions;

public class UIOptionChoiceController : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private TMP_Text _selectedChoiceText;

    [SerializeField] private Button _defaultButton;
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;

    [Header("Parameters")] 
    [SerializeField] private string[] _choiceList;

    [Header("Preferences")] 
    [SerializeField] private string _preferenceName;

    [SerializeField] private string _defaultValue;

    private int m_currentSelectedIndex;

    private void Start()
    {
        var value = PlayerPrefs.GetString(_preferenceName, _defaultValue);
        m_currentSelectedIndex = ValueToIndex(value);
        OnIndexChanged();

        _defaultButton.onClick.AddListener(OnReset);
        _leftButton.onClick.AddListener(OnLeft);
        _rightButton.onClick.AddListener(OnRight);
    }

    private void OnReset()
    {
        m_currentSelectedIndex = ValueToIndex(_defaultValue);
        OnIndexChanged();
    }

    private void OnRight()
    {
        m_currentSelectedIndex = Mod(m_currentSelectedIndex + 1, _choiceList.Length);
        OnIndexChanged();
    }

    private void OnLeft()
    {
        m_currentSelectedIndex = Mod(m_currentSelectedIndex - 1, _choiceList.Length);
        OnIndexChanged();
    }

    private void OnIndexChanged()
    {
        var value = IndexToValue(m_currentSelectedIndex);
        _selectedChoiceText.text = value;
        PlayerPrefs.SetString(_preferenceName, value);
    }

    private string IndexToValue(int index)
    {
        return _choiceList[index];
    }

    private int ValueToIndex(string value)
    {
        return _choiceList.IndexOf(value);
    }

    private int Mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}