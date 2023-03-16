using System;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected Action _OnActionComplete;
    public bool IsActive { get; protected set; }
    public int MaxEffectiveDistance { get; protected set; }
    public int ActionCost { get; protected set; }
    public Constants.SoldierActionTargetTypes TargetType { get; protected set; }

    public abstract override string ToString();
    protected abstract void DoAction(Vector3 worldPosition);

    public void DoAction(Action OnActionComplete, Vector3 worldPosition)
    {
        if (this.TargetType == Constants.SoldierActionTargetTypes.Enemy)
        {
            Vector3 soldierHeight = Vector3.up * 1.7f;
            Vector3 aimDirection = (worldPosition - transform.position).normalized;
            float shoulderOffsetAmount = 0.5f;
            Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * aimDirection * shoulderOffsetAmount;
            Vector3 from = transform.position + soldierHeight + shoulderOffset + (aimDirection * -1);
            ActionCameraController.Instance.DoShot(from, worldPosition + soldierHeight);
        }

        this._OnActionComplete = OnActionComplete;
        this.IsActive = true;
        this.DoAction(worldPosition);
    }

    protected void ActionComplete()
    {
        if (this.TargetType == Constants.SoldierActionTargetTypes.Enemy)
        {
            ActionCameraController.Instance.StopShot();
        }

        this.IsActive = false;
        this._OnActionComplete();
    }
}
