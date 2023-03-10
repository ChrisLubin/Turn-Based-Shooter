using System;
using System.Linq;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    private SoldierSelectedVisualController _visualController;
    private BaseAction[] _actions;
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
    public BaseAction[] GetActions() => this._actions;
    public BaseAction GetAction(string name) => this._actions.First(action => action.ToString() == name);
}
