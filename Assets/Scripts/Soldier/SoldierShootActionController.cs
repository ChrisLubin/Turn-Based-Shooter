using System;
using System.Threading.Tasks;
using UnityEngine;

public class SoldierShootActionController : BaseAction
{
    private State _state;
    private Vector3 _targetDirection;
    private enum State
    {
        None,
        Aiming,
        Shooting,
        Cooloff,
    }
    public Action<Vector3, int> OnShoot;

    private void Awake()
    {
        this.MaxEffectiveDistance = 5;
        this.ActionCost = 1;
        this.TargetType = Constants.SoldierActionTargetTypes.Enemy;
    }

    private void Update()
    {
        if (!this.IsActive)
        {
            return;
        }

        if (this._state == State.Aiming)
        {
            Vector3 aimDirection = (this._targetDirection - transform.position).normalized;
            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
        }
    }

    protected async override void DoAction(Vector3 worldPosition)
    {
        this._targetDirection = worldPosition;
        this._state = State.Aiming;
        // Aiming
        await Task.Delay(TimeSpan.FromSeconds(1));
        this._state = State.Shooting;

        // Shooting
        this.OnShoot?.Invoke(worldPosition, 30);
        await Task.Delay(TimeSpan.FromSeconds(.1));
        this._state = State.Cooloff;

        // Cooloff
        await Task.Delay(TimeSpan.FromSeconds(.5));

        this.ActionComplete();
    }

    public override string ToString() => Constants.SoldierActionNames.Shoot;
}
