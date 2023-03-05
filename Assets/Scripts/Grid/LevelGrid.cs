using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] private Transform _gridDebugObjectPrefab;
    private GridController _gridController;
    public static LevelGrid Instance
    {
        get; private set;
    }
    private Soldier[] _soldiers;
    private IDictionary<Soldier, GridPosition> _soldierToGridPositionMap = new Dictionary<Soldier, GridPosition>();
    private readonly int _MAX_SOLDIER_MOVE_DISTANCE = 1;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one LevelGrid! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        this._gridController = new GridController(4, 4, 2f);
    }

    private void Start()
    {
        GlobalMouse.Instance.OnLayerLeftClick += this.OnLayerLeftClick;
        this._gridController.CreateDebugObjects(this.transform, _gridDebugObjectPrefab);
        this._soldiers = GameObject.FindObjectsOfType<Soldier>();
        foreach (Soldier soldier in this._soldiers)
        {
            OnSoldierSpawn(soldier);
        }
    }

    private void Update()
    {
        foreach (Soldier soldier in this._soldiers)
        {
            bool successfullyGotFrom = this._soldierToGridPositionMap.TryGetValue(soldier, out GridPosition from);
            GridPosition to = this._gridController.GetGridPosition(soldier.transform.position);
            if (successfullyGotFrom && from != to)
            {
                ClearSoldierAtGridPosition(from);
                SetSoldierAtGridPosition(to, soldier);

                foreach (Soldier innerSoldier in this._soldiers)
                {
                    bool anotherSoldierLeftMyGridPosition = this._gridController.GetGridPosition(innerSoldier.transform.position) == from;
                    if (anotherSoldierLeftMyGridPosition)
                    {
                        SetSoldierAtGridPosition(from, innerSoldier);
                        break;
                    }
                }
            }
        }
    }

    private void OnLayerLeftClick(int layerMaskId, GameObject gameObject)
    {
        if (layerMaskId == (int)Constants.LayerMaskIds.MainFloor)
        {
            Vector3 selectedSoldierPosition = SoldiersActionController.Instance.GetSelectedSolder().transform.position;
            GridPosition from = this._gridController.GetGridPosition(selectedSoldierPosition);
            GridPosition to = this._gridController.GetGridPosition(GlobalMouse.Instance.GetFloorPosition());
            if (!this.CanSoldierMoveToGridPosition(from, to))
            {
                return;
            }

            Vector3 middleOfGridPosition = this._gridController.GetWorldPosition(to);
            SoldiersActionController.Instance.MoveSelectedSoldierToPosition(middleOfGridPosition);
        }
    }

    private void SetSoldierAtGridPosition(GridPosition gridPosition, Soldier soldier)
    {
        GridObject gridObject = this._gridController.GetGridObject(gridPosition);
        gridObject.SetSoldier(soldier);
        this._soldierToGridPositionMap[soldier] = gridPosition;
    }

    private void ClearSoldierAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = this._gridController.GetGridObject(gridPosition);
        gridObject.RemoveSoldier();
    }

    private GridPosition[] GetSurroundingGridPositions(GridPosition originalGridPosition)
    {
        List<GridPosition> surroundingGridPositions = new();

        for (int x = originalGridPosition.x - _MAX_SOLDIER_MOVE_DISTANCE; x <= originalGridPosition.x + _MAX_SOLDIER_MOVE_DISTANCE; x++)
        {
            for (int z = originalGridPosition.z - _MAX_SOLDIER_MOVE_DISTANCE; z <= originalGridPosition.z + _MAX_SOLDIER_MOVE_DISTANCE; z++)
            {
                GridPosition gridPosition = new(x, z);
                if (this._gridController.IsValidGridPosition(originalGridPosition) && originalGridPosition != gridPosition)
                {
                    surroundingGridPositions.Add(gridPosition);
                }
            }
        }

        return surroundingGridPositions.ToArray();
    }

    private bool CanSoldierMoveToGridPosition(GridPosition from, GridPosition to)
    {
        if (!this._gridController.IsValidGridPosition(to))
        {
            return false;
        }
        if (this._gridController.GetGridObject(to).HasSoldier())
        {
            return false;
        }

        return GetSurroundingGridPositions(from).Contains(to);
    }

    public void OnSoldierSpawn(Soldier soldier)
    {
        this._soldierToGridPositionMap.Add(soldier, this._gridController.GetGridPosition(soldier.transform.position));
        SetSoldierAtGridPosition(this._gridController.GetGridPosition(soldier.transform.position), soldier);
    }
}
