using NUnit.Framework.Constraints;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
public class BoardManager : MonoBehaviour 
{
    [SerializeField] private int        gridWidth;
    [SerializeField] private int        gridHeight;
    [SerializeField] private Tile[]     groundTiles;
    [SerializeField] private Tile[]     wallTiles;
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private int        foodCount = 1; // amount of foodPrefabs
    
    //public PlayerController player;
    
    private Tilemap          _tilemap;
    private CellData[,]      _boardData;
    private Grid             _grid;
    private List<Vector2Int> _emptyCellsList; // Initialise List of Vector2Int positions
    private void InitializeComponents() 
    {
        _tilemap        = GetComponentInChildren<Tilemap>();
        _boardData      = new CellData[gridWidth, gridHeight];
        _grid           = GetComponentInChildren<Grid>();
        _emptyCellsList = new List<Vector2Int>();
    }

    #region CellToWorld > Info
    /* I want this method to translate a Cell in the GameBoard to a Vector3,
     * giving a World Position to the player. The player should be able to access the cell.
    
    public Vector3 CellToWorld(Vector2Int cellIndex) {
         return _Grid.GetCellCenterWorld((Vector3Int) cellIndex);
    } 
    * Changed to Lambda expression. Lambda forces Single-Expression. */
    #endregion
    public Vector3 CellToWorld(Vector2Int cellIndex) => 
        _grid.GetCellCenterWorld((Vector3Int)cellIndex);
    
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
        return _BoardData[cellIndex.x, cellIndex.y];
     } 
     * Changed to Lambda Expression (Single Responsibility Principle). This code does the Same thing,
     * But is more Reusable: */
    #endregion
    public CellData GetCellData(Vector2Int cellIndex) =>
    IsWithinBounds(cellIndex) ? _boardData[cellIndex.x, cellIndex.y] : null;

    private bool IsWithinBounds(Vector2Int cellIndex) =>
        cellIndex.x >= 0 && cellIndex.x < gridWidth && 
        cellIndex.y >= 0 && cellIndex.y < gridHeight;

    private void GenerateFood() 
    {
        for (int i = 0; i < foodCount; i++) 
        {
            // After List is generated, pick random Cell from the generated List of Cells
            int randomIndex = Random.Range(0, _emptyCellsList.Count);
                
            // Int to store Coordinates of Random Cell in List
            Vector2Int cellCoordinate = _emptyCellsList[randomIndex];
               
            // Remove Random Cell from List  :  ( It will now be occupied by a Food object )
            _emptyCellsList.RemoveAt(randomIndex);
            
            //Retrieve data from random cell pos (bool, passable)
            CellData data = _boardData[cellCoordinate.x, cellCoordinate.y];
            GameObject newFood = Instantiate(foodPrefab);             // Instantiate
            newFood.transform.position = CellToWorld(cellCoordinate); // Transform position
            data.ContainedObject = newFood;                           // Update cell to contain new object
        }
    }
    public void Init() 
    {
        InitializeComponents();
        
        for (int y = 0; y < gridHeight; ++y) 
        {
           for(int x = 0; x < gridWidth; ++x) 
           {
               Tile tile;
               _boardData[x, y] = new CellData();
               
               if(x == 0 || y == 0 || x == gridWidth - 1 || y == gridHeight - 1) 
               {
                   tile = wallTiles[Random.Range(0, wallTiles.Length)];
                   _boardData[x, y].Passable = false;
               } 
               else 
               {
                   tile = groundTiles[Random.Range(0, groundTiles.Length)];
                   _boardData[x, y].Passable = true;
                   
                   // Passable Empty Cell -> Add to List
                   _emptyCellsList.Add(new Vector2Int(x, y));
               }
               _tilemap.SetTile(new Vector3Int(x, y, 0), tile);
           }
        }
        // Player spawns 1,1 : Occupied : Remove "Empty" Cell from List
        _emptyCellsList.Remove(new Vector2Int(1, 1));
        GenerateFood();
    }
}