using System;
using UnityEngine;

public class SoldierSpinActionController : BaseAction
{
    private float _totalSpin;
    private float _originalRotationAngle;

    private void Update()
    {
        if (!this.IsActive)
        {
            return;
        }
        if (this._totalSpin >= 360f)
        {
            this.IsActive = false;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, this._originalRotationAngle, transform.eulerAngles.z);
            this._OnActionComplete();
            return;
        }

        float addingAmount = 180f * Time.deltaTime;
        this._totalSpin += addingAmount;
        transform.eulerAngles += new Vector3(0, addingAmount, 0);
    }

    public override string ToString() => "Spin";

    public override void DoAction(Action OnActionComplete)
    {
        base.DoAction(OnActionComplete);
        this._totalSpin = 0;
        this._originalRotationAngle = transform.eulerAngles.y;
    }
}
