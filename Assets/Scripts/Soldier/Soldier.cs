using System;
using System.Linq;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    private const int _MAX_ACTION_POINTS = 2;
    [SerializeField] private bool _isEnemy;
    private SoldierSelectedVisualController _visualController;
    private BaseAction[] _actions;
    private int _actionPoints = _MAX_ACTION_POINTS;
    public bool HasActiveAction
    {
        get => this._actions.Any(action => action.IsActive);
        set => HasActiveAction = value;
    }

    private void Awake()
    {
        this._visualController = GetComponentInChildren<SoldierSelectedVisualController>();
        this._actions = GetComponents<BaseAction>();
    }

    public void SetVisual(bool showVisual) => this._visualController.SetShowVisual(showVisual);
    public string[] GetActionNames() => this._actions.Select(action => action.ToString()).ToArray();
    public int GetActionCost(string actionName) => this._actions.First(action => action.ToString() == actionName).ActionCost;
    public int GetActionEffectiveDistance(string actionName) => this._actions.First(action => action.ToString() == actionName).MaxEffectiveDistance;
    public int GetActionPoints() => this._actionPoints;
    public void ResetActionPoints() => this._actionPoints = _MAX_ACTION_POINTS;
    public bool GetIsEnemy() => this._isEnemy;

    public void DoAction(Action OnActionComplete, Vector3 worldPosition, string actionName)
    {
        BaseAction action = this._actions.First(action => action.ToString() == actionName);
        if (action.ActionCost > this._actionPoints)
        {
            return;
        }

        action.DoAction(OnActionComplete, worldPosition);
        this._actionPoints -= action.ActionCost;
    }
}
