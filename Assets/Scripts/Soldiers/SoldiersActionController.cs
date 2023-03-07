using System;
using UnityEngine;

public class SoldiersActionController : MonoBehaviour
{
    [SerializeField] private Soldier _selectedSoldier;
    public static SoldiersActionController Instance
    {
        get; private set;
    }
    public event Action OnSelectedSoldierChange;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }

        Debug.LogError("There's more than one SoldierActionController! " + transform + " - " + Instance);
        Destroy(gameObject);
    }

    private void Start()
    {
        GlobalMouse.Instance.OnLayerLeftClick += this.OnLayerLeftClick;
        this._selectedSoldier.SetVisual(true);
    }

    public void MoveSelectedSoldierToPosition(Vector3 to) => this._selectedSoldier.SetTargetPosition(to);
    public Soldier GetSelectedSolder() => this._selectedSoldier;

    private void OnLayerLeftClick(int layerMaskId, GameObject gameObject)
    {
        if (layerMaskId != (int)Constants.LayerMaskIds.Soldier)
        {
            return;
        }

        bool gotSoldier = gameObject.TryGetComponent<Soldier>(out Soldier soldier);
        if (gotSoldier)
        {
            HandleSoldierSelection(soldier);
        }
    }

    private void HandleSoldierSelection(Soldier soldier)
    {
        if (this._selectedSoldier != soldier)
        {
            this._selectedSoldier.SetVisual(false);
            this._selectedSoldier = soldier;
            this._selectedSoldier.SetVisual(true);
            this.OnSelectedSoldierChange?.Invoke();
        }
    }
}
