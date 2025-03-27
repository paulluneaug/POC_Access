using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityUtility.SceneReference;
using UnityUtility.Singletons;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    //Platformer puzzle shooter
    [SerializeField] private SceneReference[] m_gameplayScenes;

    [SerializeField] private InputActionReference m_skipMinigameAction;

    [SerializeField] private UIOptionsMenuController m_optionsMenuController;

    [SerializeField] private InputActionReference m_pauseAction;

    [NonSerialized] private int m_currentSceneIndex;
    [NonSerialized] private MiniGameManager m_currentMinigame;

    private float m_currentTimeScale = 1;

    protected override void Start()
    {
        base.Start();

        m_currentSceneIndex = -1;

        GameOptionsManager optionManager = GameOptionsManager.Instance;
        optionManager.IsWindowed.OnValueChanged += OnWindowedModeChanged;
        optionManager.GameSpeed.OnValueChanged += OnGameSpeedChanged;

        m_skipMinigameAction.action.performed += SkipMinigame;

        m_optionsMenuController.OnMenuClosed += OnOptionsMenuClosedFirstTime;
        m_optionsMenuController.OpenOptionMenu();
        m_pauseAction.action.performed += OnGamePaused;
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        Time.timeScale = m_currentTimeScale;
    }

    private void OnOptionsMenuClosedFirstTime()
    {
        m_optionsMenuController.OnMenuClosed -= OnOptionsMenuClosedFirstTime;
        m_optionsMenuController.OnMenuClosed += OnOptionsMenuClosed;
        StartGame();
    }

    private void OnOptionsMenuClosed()
    {
        ResumeGame();
    }

    private void OnGamePaused(InputAction.CallbackContext obj)
    {
        PauseGame();
    }

    private void StartGame()
    {
        Time.timeScale = m_currentTimeScale;
        LoadNextScene();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        
        if (!GameOptionsManager.ApplicationIsQuitting)
        {
            GameOptionsManager optionManager = GameOptionsManager.Instance;
            optionManager.IsWindowed.OnValueChanged -= OnWindowedModeChanged;
            optionManager.GameSpeed.OnValueChanged -= OnGameSpeedChanged;
        }

        m_skipMinigameAction.action.performed -= SkipMinigame;
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_optionsMenuController.IsOpened)
        {
            return;
        }
        
        if (m_currentMinigame != null && m_currentMinigame.RequestSceneReload)
        {
            m_currentMinigame.RequestSceneReload = false;
            LoadGameScene(m_currentSceneIndex);
        }
    }

    private async void LoadGameScene(int miniGameIndex)
    {
        if (m_currentMinigame != null)
        {
            m_currentMinigame.Dispose();
            m_currentMinigame.OnGameFinished += OnGameFinished;
        }

        await SceneManager.LoadSceneAsync(m_gameplayScenes[miniGameIndex], LoadSceneMode.Single);

        m_currentMinigame = FindFirstObjectByType<MiniGameManager>();

        m_currentMinigame.OnGameFinished += OnGameFinished;
        m_currentMinigame.StartGame();
    }

    private void LoadNextScene()
    {
        m_currentSceneIndex++;

        if (m_currentSceneIndex >= m_gameplayScenes.Length)
        {
            FinishGames();
            return;
        }

        LoadGameScene(m_currentSceneIndex);
    }

    private void SkipMinigame(InputAction.CallbackContext context)
    {
        if (m_optionsMenuController.IsOpened)
        {
            return;
        }
        LoadNextScene();
    }

    private void OnGameFinished()
    {
        LoadNextScene();
    }

    private void FinishGames() 
    {

    }

    private void OnGameSpeedChanged(float newTimeScale)
    {
        m_currentTimeScale = newTimeScale;
    }

    private void OnWindowedModeChanged(bool newValue)
    {
        Screen.fullScreen = !newValue;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
