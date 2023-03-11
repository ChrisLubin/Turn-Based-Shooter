using System;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Action _OnActionComplete;
    public bool IsActive { get; protected set; }
    public int MaxEffectiveDistance { get; protected set; }
    public int ActionCost { get; protected set; }

    public void DoAction(Action OnActionComplete, Vector3 worldPosition)
    {
        this._OnActionComplete = OnActionComplete;
        this.IsActive = true;
        this.DoAction(worldPosition);
    }

    protected abstract void DoAction(Vector3 worldPosition);

    public abstract override string ToString();
}
