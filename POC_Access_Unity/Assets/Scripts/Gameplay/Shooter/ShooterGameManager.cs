using System;
using System.Linq;
using UnityEngine;

public class ShooterGameManager : MiniGameManager
{
    [NonSerialized] private ShooterEnemy[] m_enemies;

    // Update is called once per frame
    private void Update()
    {
        if (!m_started)
        {
            return;
        }

        if (m_enemies.All(e => !e.IsAlive))
        {
            FinishGame();
        }
    }

    public override void StartGame()
    {
        base.StartGame();
        m_enemies = FindObjectsByType<ShooterEnemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
