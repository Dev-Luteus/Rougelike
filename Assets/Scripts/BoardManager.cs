using NUnit.Framework.Constraints;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
public class BoardManager : MonoBehaviour {
    public PlayerController Player;
    
    [SerializeField] private int        Width;
    [SerializeField] private int        Height;
    [SerializeField] private Tile[]     GroundTiles;
    [SerializeField] private Tile[]     WallTiles;
    [SerializeField] private GameObject FoodPrefab;
    [SerializeField] private int        FoodCount = 1; // amount of FoodPrefabs
    
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
        m_BoardData      = new CellData[Width, Height];
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
        if (cellIndex.x < 0 || cellIndex.x >= Width || 
            cellIndex.y < 0 || cellIndex.y >= Height) {
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
        cellIndex.x >= 0 && cellIndex.x < Width && 
        cellIndex.y >= 0 && cellIndex.y < Height;
    
    void GenerateFood() {
        for (int i = 0; i < FoodCount; i++) {
            // After List is generated, pick random Cell from the generated List of Cells
            int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
                
            // Int to store Coordinates of Random Cell in List
            Vector2Int cellCoordinate = m_EmptyCellsList[randomIndex];
               
            // Remove Random Cell from List  :  ( It will now be occupied by a Food object )
            m_EmptyCellsList.RemoveAt(randomIndex);
            
            //Retrieve data from random cell pos (bool, passable)
            CellData data = m_BoardData[cellCoordinate.x, cellCoordinate.y];
            GameObject newFood = Instantiate(FoodPrefab);             // Instantiate
            newFood.transform.position = CellToWorld(cellCoordinate); // Transform position
            data.ContainedObject = newFood;                           // Update cell to contain new object
        }
    }
    public void Init() {
        InitializeComponents();
        
        for (int y = 0; y < Height; ++y) {
           for(int x = 0; x < Width; ++x) {
               Tile tile;
               m_BoardData[x, y] = new CellData();
               
               if(x == 0 || y == 0 || x == Width - 1 || y == Height - 1) {
                   tile = WallTiles[Random.Range(0, WallTiles.Length)];
                   m_BoardData[x, y].Passable = false;
               } else {
                   tile = GroundTiles[Random.Range(0, GroundTiles.Length)];
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