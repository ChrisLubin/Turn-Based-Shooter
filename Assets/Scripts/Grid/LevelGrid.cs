using System.Collections.Generic;
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
            GridPosition to = GetGridPosition(soldier.transform.position);
            if (successfullyGotFrom && from != to)
            {
                ClearSoldierAtGridPosition(from);
                SetSoldierAtGridPosition(to, soldier);

                foreach (Soldier innerSoldier in this._soldiers)
                {
                    bool anotherSoldierLeftMyGridPosition = GetGridPosition(innerSoldier.transform.position) == from;
                    if (anotherSoldierLeftMyGridPosition)
                    {
                        SetSoldierAtGridPosition(from, innerSoldier);
                        break;
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown((int) Constants.MouseButtonIds.LeftClick))
        {
            bool didClickFloor = GlobalMouse.IsIntersecting((int) Constants.LayerMaskIds.MainFloor);
            if (didClickFloor)
            {
                GridPosition gridPosition = this._gridController.GetGridPosition(GlobalMouse.GetFloorPosition());
                Vector3 middleOfGridPosition = this._gridController.GetWorldPosition(gridPosition);
                SoldiersActionController.Instance.MoveSelectedSoldierToPosition(middleOfGridPosition);
            }
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

    private GridPosition GetGridPosition(Vector3 worldPosition) => this._gridController.GetGridPosition(worldPosition);

    public void OnSoldierSpawn(Soldier soldier)
    {
        this._soldierToGridPositionMap.Add(soldier, GetGridPosition(soldier.transform.position));
        SetSoldierAtGridPosition(GetGridPosition(soldier.transform.position), soldier);
    }
}
