using UnityEngine;
#region CellData > Info
    /* This class is supposed to hold Extra information about each cell in the grid
     * Currently, each cell has a X and Y position in the grid, but now they can have a Bool,
     * And other potential info like GameObjects */
    #endregion
public class CellData 
{
    public bool Passable;
    public GameObject ContainedObject;
}
