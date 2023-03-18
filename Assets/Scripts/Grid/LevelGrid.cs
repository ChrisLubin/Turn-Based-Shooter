using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] private Transform _gridTilePrefab;
    private GridController<GridTile> _gridController;
    public static LevelGrid Instance { get; private set; }
    private List<Soldier> _soldiers;
    private IDictionary<Soldier, GridTile> _soldierToGridTileMap = new Dictionary<Soldier, GridTile>();
    private List<GridTile> _gridTilesWithActiveVisual = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            this._gridController = new GridController<GridTile>(12, 15, 2f);
            return;
        }

        Debug.LogError("There's more than one LevelGrid! " + transform + " - " + Instance);
        Destroy(gameObject);
    }

    private void Start()
    {
        GlobalMouse.Instance.OnLayerLeftClick += this.OnLayerLeftClick;
        SoldiersActionController.Instance.OnSelectedActionChange += this.UpdateActiveGridTiles;
        SoldiersActionController.Instance.OnActionCompleted += this.UpdateActiveGridTiles;
        SoldiersActionController.Instance.OnSelectedSoldierHasNoActionPoints += this.ClearAllActiveGridTiles;
        TurnController.Instance.OnTurnEnd += this.OnTurnEnd;
        this._gridController.CreateGridTiles(this.transform, _gridTilePrefab);
        this._soldiers = GameObject.FindObjectsOfType<Soldier>().ToList();
        foreach (Soldier soldier in this._soldiers)
        {
            OnSoldierSpawn(soldier);
        }
        UpdateActiveGridTiles();
    }

    private void Update()
    {
        foreach (Soldier soldier in this._soldiers)
        {
            bool successfullyGotFrom = this._soldierToGridTileMap.TryGetValue(soldier, out GridTile from);
            GridTile to = this._gridController.GetGridTile(soldier.transform.position);
            if (successfullyGotFrom && from != to)
            {
                this.ClearSoldierAtGridTile(from);
                this.SetSoldierAtGridTile(to, soldier);
                this.UpdateActiveGridTiles();

                foreach (Soldier innerSoldier in this._soldiers)
                {
                    bool anotherSoldierLeftMyGridPosition = this._gridController.GetGridTile(innerSoldier.transform.position) == from;
                    if (anotherSoldierLeftMyGridPosition)
                    {
                        this.SetSoldierAtGridTile(from, innerSoldier);
                        this.UpdateActiveGridTiles();
                        break;
                    }
                }
            }
        }
    }

    private void ClearSoldierAtGridTile(GridTile gridTile) => gridTile.RemoveSoldier();
    private void OnShoot(Vector3 positionToShoot, int damage) => this._gridController.GetGridTile(positionToShoot).GetSoldier()?.TakeDamage(damage);
    public GridTile GetGridTile(Vector3 worldPosition) => this._gridController.GetGridTile(worldPosition);
    public Vector3 GetWorldPosition(GridTile gridTile) => this._gridController.GetWorldPosition(gridTile.GetGridPosition());

    private void OnTurnEnd(bool isPlayerTurn)
    {
        if (!isPlayerTurn)
        {
            this.ClearAllActiveGridTiles();
            return;
        }

        this.UpdateActiveGridTiles();
    }

    private void OnLayerLeftClick(int layerMaskId, GameObject gameObject)
    {
        if (!TurnController.Instance.GetIsPlayerTurn())
        {
            return;
        }
        if (SoldiersActionController.Instance.IsBusy)
        {
            return;
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // Hovering over UI element
            return;
        }

        Vector3 selectedSoldierPosition = SoldiersActionController.Instance.GetSelectedSolder().transform.position;
        GridPosition from = this._gridController.GetGridPosition(selectedSoldierPosition);
        GridPosition to = new();

        if (layerMaskId == (int)Constants.LayerMaskIds.MainFloor)
        {
            to = this._gridController.GetGridPosition(GlobalMouse.Instance.GetFloorPosition());
        }
        else if (layerMaskId == (int)Constants.LayerMaskIds.Soldier)
        {
            bool gotSoldier = gameObject.TryGetComponent<Soldier>(out Soldier soldier);
            if (!gotSoldier)
            {
                return;
            }

            to = this._gridController.GetGridPosition(soldier.transform.position);
        }

        if (!this.CanDoActionOnGridPosition(from, to))
        {
            return;
        }

        Vector3 middleOfGridPosition = this._gridController.GetWorldPosition(to);
        SoldiersActionController.Instance.DoAction(middleOfGridPosition);
    }

    private void SetSoldierAtGridTile(GridTile gridTile, Soldier soldier)
    {
        gridTile.SetSoldier(soldier);
        this._soldierToGridTileMap[soldier] = gridTile;
    }

    private bool CanDoActionOnGridPosition(GridPosition from, GridPosition to)
    {
        if (!this._gridController.IsValidGridPosition(to))
        {
            return false;
        }

        GridTile fromGridTile = this._gridController.GetGridTile(this._gridController.GetWorldPosition(from));
        GridTile toGridTile = this._gridController.GetGridTile(this._gridController.GetWorldPosition(to));
        int actionMaxEffectiveDistance = SoldiersActionController.Instance.GetSelectedActionEffectiveDistance();
        Constants.SoldierActionTargetTypes actionTargetType = SoldiersActionController.Instance.GetSelectedActionTargetType();

        if (this.IsOccupiedByObstacle(toGridTile))
        {
            return false;
        }

        switch (actionTargetType)
        {
            case Constants.SoldierActionTargetTypes.Self:
                return from == to;
            case Constants.SoldierActionTargetTypes.Enemy:
                if (!toGridTile.HasEnemySoldier())
                {
                    return false;
                }

                return this._gridController.GetSurroundingGridTiles(fromGridTile, actionMaxEffectiveDistance).Contains(toGridTile);
            case Constants.SoldierActionTargetTypes.EmptyTile:
                if (toGridTile.HasSoldier())
                {
                    return false;
                }

                return this._gridController.GetSurroundingGridTiles(fromGridTile, actionMaxEffectiveDistance).Contains(toGridTile);
        }

        return false;
    }

    private void OnSoldierSpawn(Soldier soldier)
    {
        soldier.OnShoot += this.OnShoot;
        soldier.OnDeath += this.OnSoldierDeath;
        this._soldierToGridTileMap.Add(soldier, this._gridController.GetGridTile(soldier.transform.position));
        SetSoldierAtGridTile(this._gridController.GetGridTile(soldier.transform.position), soldier);
    }

    private void OnSoldierDeath(Soldier soldier)
    {
        soldier.OnShoot -= this.OnShoot;
        soldier.OnDeath -= this.OnSoldierDeath;
        GridTile gridTile = this._gridController.GetGridTile(soldier.transform.position);
        this.ClearSoldierAtGridTile(gridTile);
        this._soldiers.Remove(soldier);
    }

    private void AddGridTilesActive(GridTile[] gridTiles, Constants.GridTileColor color = Constants.GridTileColor.White)
    {
        foreach (GridTile gridTile in gridTiles)
        {
            gridTile.SetVisual(true, color);
        }
        this._gridTilesWithActiveVisual.AddRange(gridTiles);
    }

    private void ClearAllActiveGridTiles()
    {
        foreach (GridTile gridTile in this._gridTilesWithActiveVisual)
        {
            gridTile.SetVisual(false);
        }
        this._gridTilesWithActiveVisual.Clear();
    }

    private void UpdateActiveGridTiles()
    {
        if (!TurnController.Instance.GetIsPlayerTurn())
        {
            return;
        }

        Soldier selectedSoldier = SoldiersActionController.Instance.GetSelectedSolder();
        string selectedActionName = SoldiersActionController.Instance.GetSelectedActionName();
        Constants.SoldierActionTargetTypes selectedActionTargetType = SoldiersActionController.Instance.GetSelectedActionTargetType();
        GridTile[] validGridTiles = this.GetValidGridTiles(selectedActionTargetType, selectedSoldier, selectedSoldier.transform.position, selectedActionName);
        ClearAllActiveGridTiles();
        Constants.GridTileColor color = Constants.GridTileColor.White;

        if (selectedSoldier.GetActionPoints() == 0)
        {
            return;
        }

        switch (selectedActionTargetType)
        {
            case Constants.SoldierActionTargetTypes.Self:
                color = Constants.GridTileColor.Blue;
                break;
            case Constants.SoldierActionTargetTypes.Enemy:
                color = Constants.GridTileColor.Red;
                GridTile selectedSoldierGridTile = this._gridController.GetGridTile(selectedSoldier.transform.position);
                int selectedActionMaxEffectiveDistance = SoldiersActionController.Instance.GetSelectedActionEffectiveDistance();
                GridTile[] surroundingGridTiles = this._gridController.GetSurroundingGridTiles(selectedSoldierGridTile, selectedActionMaxEffectiveDistance);
                GridTile[] nonValidGridTiles = surroundingGridTiles.Where(tile => !tile.HasEnemySoldier() && !this.IsOccupiedByObstacle(tile) && !this.IsObstacleBetween(selectedSoldierGridTile, tile)).ToArray();
                AddGridTilesActive(nonValidGridTiles, Constants.GridTileColor.RedSoft);
                break;
            case Constants.SoldierActionTargetTypes.EmptyTile:
                color = Constants.GridTileColor.White;
                break;
        }

        AddGridTilesActive(validGridTiles, color);
    }

    private bool IsOccupiedByObstacle(GridTile gridTile)
    {
        float rayCastOffsetDistance = 0.5f;
        return Physics.Raycast(this.GetWorldPosition(gridTile) + Vector3.down * rayCastOffsetDistance, Vector3.up, rayCastOffsetDistance * 2, (int)Constants.LayerMaskIds.Obstacle);
    }

    private bool IsObstacleBetween(GridTile fromGrid, GridTile toGrid)
    {
        float rayCastOffsetDistance = 1f;
        Vector3 fromPosition = this.GetWorldPosition(fromGrid) + Vector3.up * rayCastOffsetDistance;
        Vector3 toPosition = this.GetWorldPosition(toGrid) + Vector3.up * rayCastOffsetDistance;
        Vector3 toDirection = (toPosition - fromPosition).normalized;
        return Physics.Raycast(fromPosition, toDirection, Vector3.Distance(fromPosition, toPosition), (int)Constants.LayerMaskIds.Obstacle);
    }

    public GridTile[] GetValidGridTiles(Constants.SoldierActionTargetTypes actionTargetType, Soldier soldier, Vector3 originalPosition, string actionName)
    {
        GridTile[] validGridTiles = Array.Empty<GridTile>();
        GridTile soldierGridTile = this._gridController.GetGridTile(originalPosition);
        int actionMaxEffectiveDistance = soldier.GetActionEffectiveDistance(actionName);
        bool isPlayer = TurnController.Instance.GetIsPlayerTurn();
        GridTile[] surroundingGridTiles = this._gridController.GetSurroundingGridTiles(soldierGridTile, actionMaxEffectiveDistance);

        switch (actionTargetType)
        {
            case Constants.SoldierActionTargetTypes.Self:
                validGridTiles = new[] { soldierGridTile };
                break;
            case Constants.SoldierActionTargetTypes.Enemy:
                validGridTiles = surroundingGridTiles.Where(tile => (isPlayer ? tile.HasEnemySoldier() : tile.HasFriendlySoldier()) && !this.IsObstacleBetween(soldierGridTile, tile)).ToArray();
                break;
            case Constants.SoldierActionTargetTypes.EmptyTile:
                validGridTiles = surroundingGridTiles.Where(tile => !tile.HasSoldier()).ToArray();
                break;
        }
        validGridTiles = validGridTiles.Where(tile => !this.IsOccupiedByObstacle(tile)).ToArray();

        return validGridTiles;
    }
}
