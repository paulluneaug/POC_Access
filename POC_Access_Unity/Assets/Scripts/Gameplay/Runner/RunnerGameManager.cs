using UnityEngine;
using UnityEngine.SceneManagement;
using UnityUtility.Extensions;

public class RunnerGameManager : MonoBehaviour
{
    [SerializeField] private TriggerObject[] m_deathZoneTriggers;
    [SerializeField] private TriggerObject m_winZoneTrigger;

    [SerializeField] private RunnerPlayerController m_player;

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        m_deathZoneTriggers.ForEach(deathZone => deathZone.OnEnter += OnEnterDeathZone);
        m_winZoneTrigger.OnEnter += OnWinZoneEnter;

        m_player.StartPlayer();
    }


    private void FinishGame(bool win)
    {
        m_deathZoneTriggers.ForEach(deathZone => deathZone.OnEnter -= OnEnterDeathZone);
        m_winZoneTrigger.OnEnter -= OnWinZoneEnter;


        m_player.StopPlayer();
    }

    private void OnWinZoneEnter(Collider collider)
    {
        FinishGame(true);
    }

    private void OnEnterDeathZone(Collider collider)
    {
        if (collider.gameObject != m_player.gameObject)
        {
            return;
        }

        FinishGame(false);

        SceneManager.LoadScene(gameObject.scene.buildIndex);
    }
}
