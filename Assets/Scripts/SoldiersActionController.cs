using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldiersActionController : MonoBehaviour
{
    [SerializeField] private Soldier _selectedSoldier;

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
        _selectedSoldier = soldier;
    }
}
