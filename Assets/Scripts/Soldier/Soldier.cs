using System;
using System.Linq;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    private SoldierSelectedVisualController _visualController;
    private SoldierMoveActionController _moveActionController;
    private SoldierSpinActionController _spinActionController;
    private BaseAction[] _actions = Array.Empty<BaseAction>();
    public bool HasActiveAction
    {
        get => this._actions.Any(action => action.IsActive);
        set => HasActiveAction = value;
    }

    private void Awake()
    {
        this._visualController = GetComponentInChildren<SoldierSelectedVisualController>();
        this._moveActionController = GetComponent<SoldierMoveActionController>();
        this._spinActionController = GetComponent<SoldierSpinActionController>();
        this._actions = new BaseAction[] { this._moveActionController, this._spinActionController };
    }

    public void DoMoveAction(Action OnActionComplete, Vector3 targetPosition)
    {
        if (this.HasActiveAction)
        {
            return;
        }
        this._moveActionController.DoAction(OnActionComplete, targetPosition);
    }

    public void DoSpinAction(Action onActionComplete)
    {
        if (this.HasActiveAction)
        {
            return;
        }

        this._spinActionController.DoAction(onActionComplete);
    }

    public void SetVisual(bool showVisual) => this._visualController.SetShowVisual(showVisual);
}
