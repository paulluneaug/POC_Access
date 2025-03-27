using UnityEngine;
using UnityUtility.Extensions;

public class RunnerGameManager : MiniGameManager
{
    [SerializeField] private TriggerObject[] m_deathZoneTriggers;
    [SerializeField] private TriggerObject m_winZoneTrigger;

    [SerializeField] private RunnerPlayerController m_player;


    public override void StartGame()
    {
        base.StartGame();

        m_deathZoneTriggers.ForEach(deathZone => deathZone.OnEnter += OnEnterDeathZone);
        m_winZoneTrigger.OnEnter += OnWinZoneEnter;

        m_player.StartPlayer();
    }

    public override void Dispose()
    {
        base.Dispose();
        m_deathZoneTriggers.ForEach(deathZone => deathZone.OnEnter -= OnEnterDeathZone);
        m_winZoneTrigger.OnEnter -= OnWinZoneEnter;
    }

    protected override void FinishGame()
    {
        base.FinishGame();
        m_player.StopPlayer();
    }

    private void OnWinZoneEnter(Collider collider)
    {
        FinishGame();
    }

    private void OnEnterDeathZone(Collider collider)
    {
        if (collider.gameObject != m_player.gameObject)
        {
            return;
        }

        if (GameOptionsManager.Instance.IsInvincible)
        {
            return;
        }

        m_player.StopPlayer();
        m_requestReload = true;
    }
}
