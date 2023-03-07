using UnityEngine;

public class GridTile : MonoBehaviour
{
    private GridPosition _gridPosition;
    private GridTileSoldierController _soldierController;
    private GridTileVisualController _visualController;

    private void Awake()
    {
        this._visualController = GetComponentInChildren<GridTileVisualController>();
        this._soldierController = new GridTileSoldierController();
    }

    private void Update()
    {
        if (this._soldierController == null)
        {
            return;
        }
        this._visualController.SetText(this.ToString());
    }

    public bool HasSoldier() => this._soldierController.HasSoldier();
    public void RemoveSoldier() => this._soldierController.RemoveSoldier();
    public void SetSoldier(Soldier soldier) => this._soldierController.SetSoldier(soldier);
    public GridPosition GetGridPosition() => this._gridPosition;
    public void SetGridPosition(GridPosition gridPosition) => this._gridPosition = gridPosition;
    public GridTileSoldierController GetGridTileSoldierController() => this._soldierController;
    public void SetVisual(bool showVisual) => this._visualController.SetVisual(showVisual);
    public override string ToString() => $"{this._gridPosition}\n{this._soldierController}";

    public static bool operator ==(GridTile a, GridTile b)
    {
        GridPosition gridPositionA = a.GetGridPosition();
        GridPosition gridPositionB = b.GetGridPosition();
        return gridPositionA.x == gridPositionB.x && gridPositionA.z == gridPositionB.z;
    }

    public static bool operator !=(GridTile a, GridTile b)
    {
        GridPosition gridPositionA = a.GetGridPosition();
        GridPosition gridPositionB = b.GetGridPosition();
        return gridPositionA.x != gridPositionB.x || gridPositionA.z != gridPositionB.z;
    }
}
