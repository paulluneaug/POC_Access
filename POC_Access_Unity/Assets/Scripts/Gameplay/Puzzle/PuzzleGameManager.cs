using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityUtility.CustomAttributes;
using UnityUtility.Extensions;
using UnityUtility.MathU;

public class PuzzleGameManager : MonoBehaviour
{
    [SerializeField] private int m_pushLimit = 10000;
    [SerializeField] private float m_axisDeadZone = 0.3f;

    [Title("Inputs")]
    [SerializeField] private InputActionReference m_movePlayerAction;
    [SerializeField] private InputActionReference m_restartLevelAction;

    [Title("Puzzle Elements")]
    [SerializeField] private PuzzlePlayer m_player;

    [NonSerialized] private PuzzleTarget[] m_targets;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        m_movePlayerAction.asset.Enable();
        StartGame();
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_targets.All(target => target.HasBox()))
        {
            FinishGame(true);
            return;
        }
    }

    private void StartGame()
    {
        m_targets = FindObjectsByType<PuzzleTarget>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        m_movePlayerAction.action.performed += OnMoveActionPerformed;
        m_restartLevelAction.action.performed += RestartGame;
    }

    private void RestartGame(InputAction.CallbackContext context)
    {
        FinishGame(false);
        SceneManager.LoadScene(gameObject.scene.buildIndex);
    }

    private void FinishGame(bool win)
    {
        m_movePlayerAction.action.performed -= OnMoveActionPerformed;
        m_restartLevelAction.action.performed -= RestartGame;

        Debug.LogWarning("Win Game");
    }

    private void OnMoveActionPerformed(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
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
                            Move(puzzleElement, offset);
                            return true;
                        }
                        return false;

                    // Transparent object
                    case (false, true):
                    case (false, false):
                        Move(puzzleElement, offset);
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
            Move(puzzleElement, offset);
            return true;
        }
    }

    private void Move(PuzzleElement puzzleElement, Vector2 offset)
    {
        puzzleElement.transform.position += offset.X0Y();
    }

    private int Sign(float value)
    {
        if (value == 0.0f )//value.Between(-m_axisDeadZone, m_axisDeadZone))
        {
            return 0;
        }
        return MathUf.Sign(value);
    }
}
