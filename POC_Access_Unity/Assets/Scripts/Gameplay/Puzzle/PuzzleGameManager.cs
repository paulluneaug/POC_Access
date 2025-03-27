using System;
using System.Linq;

using UnityEngine;
using UnityEngine.InputSystem;

using UnityUtility.CustomAttributes;
using UnityUtility.Extensions;
using UnityUtility.MathU;

public class PuzzleGameManager : MiniGameManager
{
    [SerializeField] private int m_pushLimit = 10000;

    [Title("Inputs")]
    [SerializeField] private InputActionReference m_moveUpPlayerAction;
    [SerializeField] private InputActionReference m_moveRightPlayerAction;
    [SerializeField] private InputActionReference m_moveDownPlayerAction;
    [SerializeField] private InputActionReference m_moveLeftPlayerAction;

    [SerializeField] private InputActionReference m_restartLevelAction;

    [NonSerialized] private PuzzlePlayer m_player;

    [NonSerialized] private PuzzleTarget[] m_targets;


    // Update is called once per frame
    private void Update()
    {
        if (!m_started)
        {
            return;
        }
        if (m_targets.All(target => target.HasBox()))
        {
            FinishGame();
            return;
        }
    }

    public override void StartGame()
    {
        base.StartGame();
        m_player = FindFirstObjectByType<PuzzlePlayer>();
        m_targets = FindObjectsByType<PuzzleTarget>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        m_moveUpPlayerAction.action.performed += OnMoveUpPerformed;
        m_moveRightPlayerAction.action.performed += OnMoveRightPerformed;
        m_moveDownPlayerAction.action.performed += OnMoveDownPerformed;
        m_moveLeftPlayerAction.action.performed += OnMoveLeftPerformed;

        m_restartLevelAction.action.performed += RestartGame;
    }

    public override void Dispose()
    {
        base.Dispose();

        m_moveUpPlayerAction.action.performed -= OnMoveUpPerformed;
        m_moveRightPlayerAction.action.performed -= OnMoveRightPerformed;
        m_moveDownPlayerAction.action.performed -= OnMoveDownPerformed;
        m_moveLeftPlayerAction.action.performed -= OnMoveLeftPerformed;

        m_restartLevelAction.action.performed -= RestartGame;
    }

    private void OnDestroy()
    {

        m_moveUpPlayerAction.action.performed -= OnMoveUpPerformed;
        m_moveRightPlayerAction.action.performed -= OnMoveRightPerformed;
        m_moveDownPlayerAction.action.performed -= OnMoveDownPerformed;
        m_moveLeftPlayerAction.action.performed -= OnMoveLeftPerformed;

        m_restartLevelAction.action.performed -= RestartGame;
    }

    private void RestartGame(InputAction.CallbackContext context)
    {
        m_requestReload = true;
    }

    private void OnMoveUpPerformed(InputAction.CallbackContext context)
    {
        OnMovePerformed(Vector2.up);
    }

    private void OnMoveRightPerformed(InputAction.CallbackContext context)
    {
        OnMovePerformed(Vector2.right);
    }

    private void OnMoveDownPerformed(InputAction.CallbackContext context)
    {
        OnMovePerformed(Vector2.down);
    }

    private void OnMoveLeftPerformed(InputAction.CallbackContext context)
    {
        OnMovePerformed(Vector2.left);
    }

    private void OnMovePerformed(Vector2 moveInput)
    {
        if (moveInput == Vector2.zero)
        {
            return;
        }

        Vector2 moveOffset = MathUf.Abs(moveInput.x) > MathUf.Abs(moveInput.y) ?
            new Vector2(Sign(moveInput.x), 0.0f) :
            new Vector2(0.0f, Sign(moveInput.y));

        _ = TryMove(m_player, moveOffset);
    }

    private bool TryMove(PuzzleElement puzzleElement, Vector2 offset, int pushDepth = 0)
    {
        if (offset == Vector2.zero)
        {
            return false;
        }

        Vector3 raycastOrigin = puzzleElement.Center;
        Vector3 raycastDirection = offset.X0Y();

        if (Physics.Raycast(raycastOrigin, raycastDirection, out RaycastHit hitInfos, 1.0f))
        {
            Collider hitCollider = hitInfos.collider;
            if (hitCollider.TryGetComponent(out PuzzleElement hitElement))
            {
                switch ((hitElement.IsSolid(), hitElement.IsPushable()))
                {
                    // Solid unmovable object
                    case (true, false):
                        return false;

                    // Solid movable object
                    case (true, true):
                        if (pushDepth >= m_pushLimit - 1)
                        {
                            return false;
                        }

                        bool hitElementMoved = TryMove(hitElement, offset, ++pushDepth);
                        if (hitElementMoved)
                        {
                            puzzleElement.Move(offset);
                            return true;
                        }
                        return false;

                    // Transparent object
                    case (false, true):
                    case (false, false):
                        puzzleElement.Move(offset);
                        return true;

                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {
                Debug.LogError("Hit unexpected object", hitCollider.gameObject);

                return false;
            }
        }
        else
        {
            puzzleElement.Move(offset);
            return true;
        }
    }

    private int Sign(float value)
    {
        if (value == 0.0f)//value.Between(-m_axisDeadZone, m_axisDeadZone))
        {
            return 0;
        }
        return MathUf.Sign(value);
    }
}
