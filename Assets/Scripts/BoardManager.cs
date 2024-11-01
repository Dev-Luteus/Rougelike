using NUnit.Framework.Constraints;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
public class BoardManager : MonoBehaviour {
    public PlayerController Player;
    
    [SerializeField] private int        gridWidth;
    [SerializeField] private int        gridHeight;
    [SerializeField] private Tile[]     groundTiles;
    [SerializeField] private Tile[]     wallTiles;
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private int        foodCount = 1; // amount of foodPrefabs
    
    #region CellData > Info
    /* This class is supposed to hold Extra information about each cell in the grid
     * Currently, each cell has a X and Y position in the grid, but now they can have a Bool,
     * And other potential info like GameObjects */
    #endregion
    public class CellData {
        public bool Passable;
        public GameObject ContainedObject;
    }
    private Tilemap          m_Tilemap;
    private CellData[,]      m_BoardData;
    private Grid             m_Grid;
    private List<Vector2Int> m_EmptyCellsList; // Initialise List of Vector2Int positions
    private void InitializeComponents() {
        m_Tilemap        = GetComponentInChildren<Tilemap>();
        m_BoardData      = new CellData[gridWidth, gridHeight];
        m_Grid           = GetComponentInChildren<Grid>();
        m_EmptyCellsList = new List<Vector2Int>();
    }

    #region CellToWorld > Info
    /* I want this method to translate a Cell in the GameBoard to a Vector3,
     * giving a World Position to the player. The player should be able to access the cell.
    
    public Vector3 CellToWorld(Vector2Int cellIndex) {
         return m_Grid.GetCellCenterWorld((Vector3Int) cellIndex);
    } 
    * Changed to Lambda expression. Lambda forces Single-Expression. */
    #endregion
    public Vector3 CellToWorld(Vector2Int cellIndex) => 
        m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);
    
    #region CellData/GetCellData > Info
    /* Check Cell coordinate in the Array, Return Null if player is moving into a WALL,
     * Also making sure the player movement stays inside of the defined space. Basically,
     * Safeguard against a OutOfBoundsException by prevent access outside the board. 
     * Return Null to say, no cell exists in this direction. If exists valid, if not no.
     * If its Within bounds, return X and Y position,
     public CellData GetCellData(Vector2Int cellIndex)
     {
        if (cellIndex.x < 0 || cellIndex.x >= gridWidth || 
            cellIndex.y < 0 || cellIndex.y >= gridHeight) {
            return null;
        }
        return m_BoardData[cellIndex.x, cellIndex.y];
     } 
     * Changed to Lambda Expression (Single Responsibility Principle). This code does the Same thing,
     * But is more Reusable: */
    #endregion
    public CellData GetCellData(Vector2Int cellIndex) =>
    IsWithinBounds(cellIndex) ? m_BoardData[cellIndex.x, cellIndex.y] : null;

    private bool IsWithinBounds(Vector2Int cellIndex) =>
        cellIndex.x >= 0 && cellIndex.x < gridWidth && 
        cellIndex.y >= 0 && cellIndex.y < gridHeight;
    
    void GenerateFood() {
        for (int i = 0; i < foodCount; i++) {
            // After List is generated, pick random Cell from the generated List of Cells
            int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
                
            // Int to store Coordinates of Random Cell in List
            Vector2Int cellCoordinate = m_EmptyCellsList[randomIndex];
               
            // Remove Random Cell from List  :  ( It will now be occupied by a Food object )
            m_EmptyCellsList.RemoveAt(randomIndex);
            
            //Retrieve data from random cell pos (bool, passable)
            CellData data = m_BoardData[cellCoordinate.x, cellCoordinate.y];
            GameObject newFood = Instantiate(foodPrefab);             // Instantiate
            newFood.transform.position = CellToWorld(cellCoordinate); // Transform position
            data.ContainedObject = newFood;                           // Update cell to contain new object
        }
    }
    public void Init() {
        InitializeComponents();
        
        for (int y = 0; y < gridHeight; ++y) {
           for(int x = 0; x < gridWidth; ++x) {
               Tile tile;
               m_BoardData[x, y] = new CellData();
               
               if(x == 0 || y == 0 || x == gridWidth - 1 || y == gridHeight - 1) {
                   tile = wallTiles[Random.Range(0, wallTiles.Length)];
                   m_BoardData[x, y].Passable = false;
               } else {
                   tile = groundTiles[Random.Range(0, groundTiles.Length)];
                   m_BoardData[x, y].Passable = true;
                   
                   // Passable Empty Cell -> Add to List
                   m_EmptyCellsList.Add(new Vector2Int(x, y));
               }
               m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
           }
        }
        // Player spawns 1,1 : Occupied : Remove "Empty" Cell from List
        m_EmptyCellsList.Remove(new Vector2Int(1, 1));
        GenerateFood();
    }
}