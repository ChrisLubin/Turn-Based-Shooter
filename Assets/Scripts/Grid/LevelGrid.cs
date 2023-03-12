using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] private Transform _gridTilePrefab;
    private GridController _gridController;
    public static LevelGrid Instance { get; private set; }
    private Soldier[] _soldiers;
    private IDictionary<Soldier, GridTile> _soldierToGridTileMap = new Dictionary<Soldier, GridTile>();
    private GridTile[] _gridTilesWithActiveVisual = Array.Empty<GridTile>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            this._gridController = new GridController(7, 7, 2f);
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
        SoldiersActionController.Instance.OnShoot += this.OnShoot;
        TurnController.Instance.OnTurnEnd += this.OnTurnEnd;
        this._gridController.CreateGridTiles(this.transform, _gridTilePrefab);
        this._soldiers = GameObject.FindObjectsOfType<Soldier>();
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
                ClearSoldierAtGridTile(from);
                SetSoldierAtGridTile(to, soldier);
                UpdateActiveGridTiles();

                foreach (Soldier innerSoldier in this._soldiers)
                {
                    bool anotherSoldierLeftMyGridPosition = this._gridController.GetGridTile(innerSoldier.transform.position) == from;
                    if (anotherSoldierLeftMyGridPosition)
                    {
                        SetSoldierAtGridTile(from, innerSoldier);
                        UpdateActiveGridTiles();
                        break;
                    }
                }
            }
        }
    }

    private void ClearSoldierAtGridTile(GridTile gridTile) => gridTile.RemoveSoldier();
    private void OnShoot(Vector3 positionToShoot, int damage) => this._gridController.GetGridTile(positionToShoot).GetSoldier().TakeDamage(damage);

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
        if (layerMaskId != (int)Constants.LayerMaskIds.MainFloor)
        {
            return;
        }

        Vector3 selectedSoldierPosition = SoldiersActionController.Instance.GetSelectedSolder().transform.position;
        GridPosition from = this._gridController.GetGridPosition(selectedSoldierPosition);
        GridPosition to = this._gridController.GetGridPosition(GlobalMouse.Instance.GetFloorPosition());
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

        int actionMaxEffectiveDistance = SoldiersActionController.Instance.GetSelectedActionEffectiveDistance();
        string actionTargetType = SoldiersActionController.Instance.GetSelectedActionTargetType();
        GridTile fromGridTile = this._gridController.GetGridTile(this._gridController.GetWorldPosition(from));
        GridTile toGridTile = this._gridController.GetGridTile(this._gridController.GetWorldPosition(to));

        switch (actionTargetType)
        {
            case Constants.SoldierActionTargetTypes.Self:
                return from == to;
            case Constants.SoldierActionTargetTypes.Enemy:
                if (!toGridTile.HasEnemySoldier())
                {
                    return false;
                }

                return this._gridController.GetSurroundingGridTiles(fromGridTile, actionMaxEffectiveDistance, true).Contains(toGridTile);
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
        this._soldierToGridTileMap.Add(soldier, this._gridController.GetGridTile(soldier.transform.position));
        SetSoldierAtGridTile(this._gridController.GetGridTile(soldier.transform.position), soldier);
    }

    private void SetGridTilesActive(GridTile[] gridTiles)
    {
        foreach (GridTile gridTile in gridTiles)
        {
            gridTile.SetVisual(true);
        }
        this._gridTilesWithActiveVisual = gridTiles;
    }

    private void ClearAllActiveGridTiles()
    {
        foreach (GridTile gridTile in this._gridTilesWithActiveVisual)
        {
            gridTile.SetVisual(false);
        }
        this._gridTilesWithActiveVisual = Array.Empty<GridTile>();
    }

    private void UpdateActiveGridTiles()
    {
        Soldier selectedSoldier = SoldiersActionController.Instance.GetSelectedSolder();
        string selectedActionTargetType = SoldiersActionController.Instance.GetSelectedActionTargetType();
        int selectedActionMaxEffectiveDistance = SoldiersActionController.Instance.GetSelectedActionEffectiveDistance();
        GridTile selectedSoldierGridTile = this._gridController.GetGridTile(selectedSoldier.transform.position);
        GridTile[] squareSurroundingGridTiles = this._gridController.GetSurroundingGridTiles(selectedSoldierGridTile, selectedActionMaxEffectiveDistance);
        GridTile[] validGridTiles = Array.Empty<GridTile>();
        ClearAllActiveGridTiles();

        if (selectedSoldier.GetActionPoints() == 0)
        {
            SetGridTilesActive(Array.Empty<GridTile>());
            return;
        }

        switch (selectedActionTargetType)
        {
            case Constants.SoldierActionTargetTypes.Self:
                validGridTiles = new[] { selectedSoldierGridTile };
                break;
            case Constants.SoldierActionTargetTypes.Enemy:
                GridTile[] circularSurroundingGridTiles = this._gridController.GetSurroundingGridTiles(selectedSoldierGridTile, selectedActionMaxEffectiveDistance, true);
                validGridTiles = circularSurroundingGridTiles.Where(tile => tile.HasEnemySoldier()).ToArray();
                break;
            case Constants.SoldierActionTargetTypes.EmptyTile:
                validGridTiles = squareSurroundingGridTiles.Where(tile => !tile.HasSoldier()).ToArray();
                break;
        }

        SetGridTilesActive(validGridTiles);
    }
}
