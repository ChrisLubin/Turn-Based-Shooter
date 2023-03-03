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

    private void Update()
    {
        if (Input.GetMouseButtonDown((int) Constants.MouseButtonIds.LeftClick)) 
        {
            bool clickedSoldier = GlobalMouse.TryGetIntersectingComponent<Soldier>((int) Constants.LayerMaskIds.Soldier, out Soldier soldier);
            bool clickedFloor = GlobalMouse.IsIntersecting((int) Constants.LayerMaskIds.MainFloor);
            if (clickedSoldier)
            {
                HandleSoldierSelection(soldier);
            }
            else if (clickedFloor)
            {
                HandleFloorClicked();
            }
        }
    }

    private void HandleFloorClicked()
    {
        _selectedSoldier.SetTargetPosition(GlobalMouse.GetFloorPosition());
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
