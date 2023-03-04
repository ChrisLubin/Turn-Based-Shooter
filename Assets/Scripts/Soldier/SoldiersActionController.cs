using System;
using UnityEngine;

public class SoldiersActionController : MonoBehaviour
{
    [SerializeField] private Soldier _selectedSoldier;
    public event Action<Soldier> OnSelectedSoldierChange;
    public static SoldiersActionController Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one SoldierActionController! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        GlobalMouse.Instance.OnLayerLeftClick += this.OnLayerLeftClick;
    }

    private void OnLayerLeftClick(int layerMaskId, GameObject gameObject)
    {
        if (layerMaskId == (int)Constants.LayerMaskIds.Soldier)
        {
            bool gotSoldier = gameObject.TryGetComponent<Soldier>(out Soldier soldier);
            if (gotSoldier)
            {
                HandleSoldierSelection(soldier);
            }
        }
    }

    public void MoveSelectedSoldierToPosition(Vector3 to)
    {
        this._selectedSoldier.SetTargetPosition(to);
    }

    private void HandleSoldierSelection(Soldier soldier)
    {
        if (this._selectedSoldier != soldier)
        {
            this._selectedSoldier = soldier;
            OnSelectedSoldierChange?.Invoke(soldier);
        }
    }

    public Soldier GetSelectedSoldier()
    {
        return this._selectedSoldier;
    }
}
