using System;
using System.Linq;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    private const int _MAX_ACTION_POINTS = 2;
    [SerializeField] private bool _isEnemy;
    private SoldierSelectedVisualController _visualController;
    private SoldierHealthController _healthController;
    private BaseAction[] _actions;
    private int _actionPoints = _MAX_ACTION_POINTS;
    public bool HasActiveAction
    {
        get => this._actions.Any(action => action.IsActive);
        set => HasActiveAction = value;
    }
    public Action<Vector3, int> OnShoot;
    public Action<Soldier> OnDeath;
    public Action<int> OnActionPointsChange;

    private void Awake()
    {
        this._visualController = GetComponentInChildren<SoldierSelectedVisualController>();
        this._healthController = GetComponent<SoldierHealthController>();
        this._actions = GetComponents<BaseAction>();
        SoldierShootActionController shootAction = GetComponent<SoldierShootActionController>();
        shootAction.OnShoot += (Vector3 positionToShoot, int damageAmount) => this.OnShoot?.Invoke(positionToShoot, damageAmount);
        this._healthController.OnDeath += this.DestroySoldier;
    }

    public void SetVisual(bool showVisual) => this._visualController.SetShowVisual(showVisual);
    public string[] GetActionNames() => this._actions.Select(action => action.ToString()).ToArray();
    public int GetActionCost(string actionName) => this._actions.First(action => action.ToString() == actionName).ActionCost;
    public int GetActionEffectiveDistance(string actionName) => this._actions.First(action => action.ToString() == actionName).MaxEffectiveDistance;
    public string GetActionTargetType(string actionName) => this._actions.First(action => action.ToString() == actionName).TargetType;
    public int GetActionPoints() => this._actionPoints;
    public bool GetIsEnemy() => this._isEnemy;
    public void TakeDamage(int damageAmount) => this._healthController.TakeDamage(damageAmount);

    public void ResetActionPoints()
    {
        this._actionPoints = _MAX_ACTION_POINTS;
        this.OnActionPointsChange?.Invoke(this._actionPoints);
    }

    private void DestroySoldier()
    {
        this.OnDeath?.Invoke(this);
        Destroy(gameObject);
    }

    public void DoAction(Action OnActionComplete, Vector3 worldPosition, string actionName)
    {
        BaseAction action = this._actions.First(action => action.ToString() == actionName);
        if (action.ActionCost > this._actionPoints)
        {
            return;
        }

        action.DoAction(OnActionComplete, worldPosition);
        this._actionPoints -= action.ActionCost;
        this.OnActionPointsChange?.Invoke(this._actionPoints);
    }
}
