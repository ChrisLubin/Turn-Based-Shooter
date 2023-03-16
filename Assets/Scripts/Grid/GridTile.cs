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
    public bool HasFriendlySoldier() => this._soldierController.HasFriendlySoldier();
    public bool HasEnemySoldier() => this._soldierController.HasEnemySoldier();
    public void RemoveSoldier() => this._soldierController.RemoveSoldier();
    public Soldier GetSoldier() => this._soldierController.GetSoldier();
    public void SetSoldier(Soldier soldier) => this._soldierController.SetSoldier(soldier);
    public GridPosition GetGridPosition() => this._gridPosition;
    public void SetGridPosition(GridPosition gridPosition) => this._gridPosition = gridPosition;
    public void SetVisual(bool showVisual, Constants.GridTileColor color = Constants.GridTileColor.White) => this._visualController.SetVisual(showVisual, color);
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
