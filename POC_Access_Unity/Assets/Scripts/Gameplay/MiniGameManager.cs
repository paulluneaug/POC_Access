using System;
using UnityEngine;

public abstract class MiniGameManager : MonoBehaviour
{
    public event Action OnGameFinished;

    public bool RequestSceneReload => m_requestReload;

    protected bool m_started = false;
    protected bool m_requestReload = false;


    public virtual void StartGame()
    {
        m_started = true;
    }

    public virtual void Dispose()
    {
        m_started = false;
    }

    protected virtual void FinishGame()
    {
        OnGameFinished?.Invoke();
    }
}
