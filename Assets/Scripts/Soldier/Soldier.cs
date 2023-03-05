using UnityEngine;

public class Soldier : MonoBehaviour
{
    private SoldierSelectedVisualController _solderSelectedVisualController;
    private SoldierMoveActionController _solderMoveActionController;

    private void Awake()
    {
        this._solderSelectedVisualController = GetComponentInChildren<SoldierSelectedVisualController>();
        this._solderMoveActionController = GetComponent<SoldierMoveActionController>();
    }

    public void SetTargetPosition(Vector3 targetPosition) => this._solderMoveActionController.SetTargetPosition(targetPosition);
    public void SetVisual(bool showVisual) => this._solderSelectedVisualController.SetShowVisual(showVisual);
}
