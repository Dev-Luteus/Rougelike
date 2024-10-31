using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    private BoardManager m_Board;
    private Vector2Int m_CellPosition;
    private bool isMoving = false;
    private Vector2Int targetCellPosition;
    
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
    void Update() {
        Vector2 moveInput = playerMovement.ReadValue<Vector2>();

        if (!isMoving && moveInput != Vector2.zero) {
            Vector2Int newCellTarget = m_CellPosition;

            if (moveInput.y > 0) {
                newCellTarget.y += 1;
            }
            else if (moveInput.y < 0) {
                newCellTarget.y -= 1;
            }
            else if (moveInput.x > 0) {
                newCellTarget.x += 1;
            }
            else if (moveInput.x < 0) {
                newCellTarget.x -= 1;
            }

            BoardManager.CellData cellData = m_Board.GetCellData(newCellTarget);
            if (cellData != null && cellData.Passable) {
                isMoving = true;
                GameManager.Instance.TurnManager.Tick();
                targetCellPosition = newCellTarget;
            }
        }

        if (isMoving) {
            // Move the player towards the target cell position over multiple frames
            transform.position = Vector3.Lerp(transform.position, m_Board.CellToWorld(targetCellPosition), Time.deltaTime * moveSpeed);

            if (Vector3.Distance(transform.position, m_Board.CellToWorld(targetCellPosition)) < 0.01f) {
                isMoving = false;
                m_CellPosition = targetCellPosition;
            }
        }
    }
}