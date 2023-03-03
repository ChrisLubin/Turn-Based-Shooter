
using UnityEngine;

public class GridObject
{
    private GridController _parentController;
    private GridPosition _position;

    public GridObject(GridController gridController, GridPosition gridPosition)
    {
        this._parentController = gridController;
        this._position = gridPosition;
    }

    public override string ToString()
    {
        return this._position.ToString();
    }
}
