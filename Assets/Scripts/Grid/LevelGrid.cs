using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] private Transform _gridDebugObjectPrefab;
    private GridController _gridController;
    public static LevelGrid Instance
    {
        get; private set;
    }

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
    }

    public Soldier GetSoldierAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = this._gridController.GetGridObject(gridPosition);
        return gridObject.GetSoldier();
    }

    public void SetSoldierAtGridPosition(GridPosition gridPosition, Soldier soldier)
    {
        GridObject gridObject = this._gridController.GetGridObject(gridPosition);
        gridObject.SetSoldier(soldier);
    }

    public void ClearSoldierAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = this._gridController.GetGridObject(gridPosition);
        gridObject.RemoveSoldier();
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => this._gridController.GetGridPosition(worldPosition);

    public void OnSoldierMovedGridPosition(Soldier soldier, GridPosition from, GridPosition to)
    {
        ClearSoldierAtGridPosition(from);
        SetSoldierAtGridPosition(to, soldier);
        soldier.SetGridPosition(to);
    }
}
