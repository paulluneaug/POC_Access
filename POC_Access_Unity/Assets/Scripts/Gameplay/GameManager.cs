using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityUtility.SceneReference;

public class GameManager : MonoBehaviour
{

    //Platformer puzzle shooter
    [SerializeField] private SceneReference[] m_gameplayScenes;


    [NonSerialized] private int m_currentSceneIndex;
    [NonSerialized] private MiniGameManager m_currentMinigame;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        m_currentSceneIndex = -1;
        LoadNextScene();

        GameOptionsManager optionManager = GameOptionsManager.Instance;
        optionManager.IsWindowed.OnValueChanged += OnWindowedModeChanged;
        optionManager.GameSpeed.OnValueChanged += OnGameSpeedChanged;
    }

    private void OnDestroy()
    {
        GameOptionsManager optionManager = GameOptionsManager.Instance;
        optionManager.IsWindowed.OnValueChanged -= OnWindowedModeChanged;
        optionManager.GameSpeed.OnValueChanged -= OnGameSpeedChanged;
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_currentMinigame != null && m_currentMinigame.RequestSceneReload)
        {
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

    private void OnGameFinished()
    {
        LoadNextScene();
    }

    private void FinishGames() 
    {

    }

    private void OnGameSpeedChanged(float newTimeScale)
    {
        Time.timeScale = newTimeScale;
    }

    private void OnWindowedModeChanged(bool newValue)
    {
        Screen.fullScreen = !newValue;
    }

}
