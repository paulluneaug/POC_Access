using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

using UnityUtility.MathU;

public class OptionsMenuController : MonoBehaviour
{
    [SerializeField] private CanvasGroup m_menuOptions;
    [SerializeField] private InputActionReference m_pauseAction;
    [SerializeField] private Button m_saveButton;
    [SerializeField] private Button m_defaultButton;
    [SerializeField] private Button m_closeButton;
    [SerializeField] private Button m_quitButton;
    [SerializeField] private AudioMixer m_mixer;
    [Header("Options")]
    [SerializeField] private UIAbstractDefaultable[] m_options;

    [SerializeField] private UIAbstractOption<string> m_optionInvincibility;
    [SerializeField] private UIAbstractOption<float> m_optionVolumeGlobal;
    [SerializeField] private UIAbstractOption<float> m_optionVolumeSFX;
    [SerializeField] private UIAbstractOption<float> m_optionVolumeMusic;
    [SerializeField] private UIAbstractOption<float> m_optionGameSpeed;
    [SerializeField] private UIAbstractOption<string> m_optionScreenMode;

    private bool m_isOpened;
    
    private void Start()
    {
        m_pauseAction.action.performed += OnPause;
        m_saveButton.onClick.AddListener(OnSaveButtonClicked);
        m_defaultButton.onClick.AddListener(OnDefaultButtonClicked);
        m_closeButton.onClick.AddListener(OnCloseButtonClicked);
        m_quitButton.onClick.AddListener(OnQuitButtonClicked);
        
        m_optionInvincibility.OnValueChangedEvent += OptionInvincibilityChanged;
        m_optionVolumeGlobal.OnValueChangedEvent += OptionVolumeGlobalChanged;
        m_optionVolumeSFX.OnValueChangedEvent += OptionVolumeSFXChanged;
        m_optionVolumeMusic.OnValueChangedEvent += OptionVolumeMusicChanged;
        m_optionGameSpeed.OnValueChangedEvent += OptionGameSpeedChanged;
        m_optionScreenMode.OnValueChangedEvent += OptionScreenModeChanged;

    }


    private void OptionInvincibilityChanged(string value)
    {
        GameOptionsManager.Instance.IsInvincible = value switch
        {
            "Enabled" => true,
            _ => false
        };
    }
    
    private void OptionVolumeGlobalChanged(float value)
    {
        m_mixer.SetFloat("GlobalVolume", value.Remap(0, 100, -80, 0));
    }

    private void OptionVolumeSFXChanged(float value)
    {
        m_mixer.SetFloat("SFXVolume", value.Remap(0, 100, -80, 0));
    }

    private void OptionVolumeMusicChanged(float value)
    {
        m_mixer.SetFloat("MusicVolume", value.Remap(0, 100, -80, 0));
    }

    private void OptionGameSpeedChanged(float value)
    {
        GameOptionsManager.Instance.GameSpeed.Value = value;
    }

    private void OptionScreenModeChanged(string value)
    {
        GameOptionsManager.Instance.IsWindowed.Value = value switch
        {
            "Windowed" => true,
            _ => false
        };
    }
    
    private void OnPause(InputAction.CallbackContext ctx)
    {
        if (m_isOpened)
        {
            return;
        }

        OpenOptionMenu();
    }

    public void OpenOptionMenu()
    {
        m_menuOptions.alpha = 1;
        m_menuOptions.interactable = true;
        m_menuOptions.blocksRaycasts = true;
        m_isOpened = true;
    }

    private void OnSaveButtonClicked()
    {
    }

    private void OnDefaultButtonClicked()
    {
        foreach (var option in m_options)
        {
            option.SetDefault();
        }
    }

    public void CloseOptionMenu()
    {
        m_menuOptions.alpha = 0;
        m_menuOptions.interactable = false;
        m_menuOptions.blocksRaycasts = false;
        m_isOpened = false;
    }

    private void OnCloseButtonClicked()
    {
        CloseOptionMenu();
    }

    private void OnQuitButtonClicked()
    {
        GameManager.Instance.QuitGame();
    }
}
