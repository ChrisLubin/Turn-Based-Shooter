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
            this._gridController = new GridController(4, 4, 2f);
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

    private bool HasSoldier(GridTile gridTile) => gridTile.HasSoldier();
    private void ClearSoldierAtGridTile(GridTile gridTile) => gridTile.RemoveSoldier();

    private void OnLayerLeftClick(int layerMaskId, GameObject gameObject)
    {
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
        int selectedActionMaxEffectiveDistance = SoldiersActionController.Instance.GetSelectedActionEffectiveDistance();
        if (!this._gridController.IsValidGridPosition(to))
        {
            return false;
        }
        if (this._gridController.GetGridTileSoldierController(to).HasSoldier() && selectedActionMaxEffectiveDistance != 0)
        {
            return false;
        }
        GridTile fromGridTile = this._gridController.GetGridTile(this._gridController.GetWorldPosition(from));
        GridTile toGridTile = this._gridController.GetGridTile(this._gridController.GetWorldPosition(to));

        if (selectedActionMaxEffectiveDistance == 0)
        {
            // Action can only be done on soldier's current position
            GridPosition soldiersGridPosition = this._gridController.GetGridPosition(SoldiersActionController.Instance.GetSelectedSolder().transform.position);
            return soldiersGridPosition == to;
        }

        return this._gridController.GetSurroundingGridTiles(fromGridTile, selectedActionMaxEffectiveDistance).Contains(toGridTile);
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
        Array.Clear(this._gridTilesWithActiveVisual, 0, this._gridTilesWithActiveVisual.Length);
    }

    private void UpdateActiveGridTiles()
    {
        Soldier selectedSoldier = SoldiersActionController.Instance.GetSelectedSolder();
        GridTile selectedSoldierGridTile = this._gridController.GetGridTile(selectedSoldier.transform.position);
        ClearAllActiveGridTiles();
        int selectedActionMaxEffectiveDistance = SoldiersActionController.Instance.GetSelectedActionEffectiveDistance();

        if (selectedSoldier.GetActionPoints() == 0)
        {
            SetGridTilesActive(Array.Empty<GridTile>());
            return;
        }
        if (selectedActionMaxEffectiveDistance == 0)
        {
            // Action can only be done on soldier's current position
            SetGridTilesActive(new[] { selectedSoldierGridTile });
            return;
        }

        GridTile[] surroundingGridTiles = this._gridController.GetSurroundingGridTiles(selectedSoldierGridTile, selectedActionMaxEffectiveDistance);
        SetGridTilesActive(surroundingGridTiles.Where(pos => !HasSoldier(pos)).ToArray());
    }
}
