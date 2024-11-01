using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    private BoardManager m_Board;
    private Vector2Int   m_CellPosition;
    private bool         m_isMoving = false;
    private Vector2Int   m_targetCellPosition;
    
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private InputAction playerMovement;
    
    // [SerializeField] private InputAction playerAttack;
    // [SerializeField] private InputAction playerInteract;

    private void OnEnable() => playerMovement.Enable();
    private void OnDisable() => playerMovement.Disable();
    public void Spawn(BoardManager boardManager, Vector2Int cell) {
        m_Board = boardManager;
        m_CellPosition = cell;
        transform.position = m_Board.CellToWorld(cell);
    }

    #region HandleMovementInput > Info
    /* I want this method to interpret Input and Determine Target Cell.
     * If no movement is made : Do nothing.
     * TryMove in the Determined Direction (Check Next Region) */
    #endregion
    private void HandleMovementInput() {
        Vector2 moveInput = playerMovement.ReadValue<Vector2>();
        if (moveInput == Vector2.zero) return;
        
        Vector2Int movementDirection = MovementDirection(moveInput);
        TryMove(movementDirection);
    }
    #region MovementDirection > Info
    /* I want this method to interpret and return the desired Input Direction
     * Axis of larger magnitude = Return Value
     * So if we're 30 degrees larger on one axis, default to that axis.
     * Unity's new input system SHOULD do this by default however..*/
    #endregion
    private Vector2Int MovementDirection(Vector2 moveInput) 
    {
        if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y)) {
            return moveInput.x > 0 ? Vector2Int.right : Vector2Int.left;
        } else {
            return moveInput.y > 0 ? Vector2Int.up : Vector2Int.down;
        }
    }
    #region TryMove > Info
    /* I want this method to try and move the player in the desired direction.
     * It calculates a New potential cell, checks if cell is valid and Passable,
     * Starts the Movement, and ticks the Turn. */
    #endregion
    private void TryMove(Vector2Int direction) {
        Vector2Int newCellTarget = m_CellPosition + direction;
        BoardManager.CellData cellData = m_Board.GetCellData(newCellTarget);
        
        if (cellData != null && cellData.Passable) {
            m_isMoving = true;
            m_targetCellPosition = newCellTarget;
            GameManager.Instance.TurnManager.Tick();
        }
    }
    #region MovePlayer > Info
    /* I want this method to actually Move the player by changing its Transform.Position.
     * It Moves the players position over multiple frames using a Vector3.Lerp,
     * then Stops movement upon reaching the Target cell. Change float value for Lerp accuracy*/
    #endregion
    private void MovePlayer() {
        Vector3 targetCell = m_Board.CellToWorld(m_targetCellPosition);
        transform.position = Vector3.Lerp(transform.position, targetCell, Time.deltaTime * moveSpeed);

        if (Vector3.Distance(transform.position, targetCell) < 0.01f) {
            m_isMoving = false;
            m_CellPosition = m_targetCellPosition;
        }
    }
    private void Update() {
        if (!m_isMoving) {
            HandleMovementInput();
        }
        if (m_isMoving) {
            MovePlayer();
        }
    }
}