using System;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Action _OnActionComplete;
    public bool IsActive { get; protected set; }

    public virtual void DoAction(Action OnActionComplete)
    {
        this._OnActionComplete = OnActionComplete;
        this.IsActive = true;
    }

    public abstract override string ToString();
}
