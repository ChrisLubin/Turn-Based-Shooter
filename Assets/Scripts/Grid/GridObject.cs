
using UnityEngine;

public class GridObject
{
    private GridController _parentController;
    private GridPosition _position;
    private Soldier _soldier;

    public GridObject(GridController gridController, GridPosition gridPosition)
    {
        this._parentController = gridController;
        this._position = gridPosition;
    }

    public override string ToString()
    {
        return $"{this._position.ToString()}\n{this._soldier}";
    }

    public void SetSoldier(Soldier soldier)
    {
        this._soldier = soldier;
    }

    public void RemoveSoldier()
    {
        this._soldier = null;
    }

    public Soldier GetSoldier()
    {
        return this._soldier;
    }
}
