using UnityEngine;

public class Soldier : MonoBehaviour
{
    private SoldierSelectedVisualController _visualController;
    private SoldierMoveActionController _moveActionController;

    private void Awake()
    {
        this._visualController = GetComponentInChildren<SoldierSelectedVisualController>();
        this._moveActionController = GetComponent<SoldierMoveActionController>();
    }

    public void SetTargetPosition(Vector3 targetPosition) => this._moveActionController.SetTargetPosition(targetPosition);
    public void SetVisual(bool showVisual) => this._visualController.SetShowVisual(showVisual);
}
