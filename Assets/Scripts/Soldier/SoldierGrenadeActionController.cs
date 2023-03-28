using System;
using System.Threading.Tasks;
using UnityEngine;

public class SoldierGrenadeActionController : BaseAction
{
    private Vector3 _targetPosition;
    private enum State
    {
        None,
        Aiming,
        Shooting,
        Cooloff,
    }
    private State _state;
    [SerializeField] private Transform _grenadePrefab;
    [SerializeField] private Transform _throwPoint;

    private void Awake()
    {
        this.MaxEffectiveDistance = 6;
        this.ActionCost = 1;
        this.TargetType = Constants.SoldierActionTargetTypes.Any;
    }

    private void Update()
    {
        if (!this.IsActive)
        {
            return;
        }

        if (this._state == State.Aiming)
        {
            Vector3 aimDirection = (this._targetPosition - transform.position).normalized;
            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
        }
    }

    public override string ToString() => Constants.SoldierActionNames.Grenade;

    private void InvokeGrenadeAction()
    {
        ScreenShakeController.Instance.Shake(2);
    }

    protected async override void DoAction(Vector3 worldPosition)
    {
        // Aiming
        this._state = State.Aiming;
        this._targetPosition = worldPosition;
        await Task.Delay(TimeSpan.FromSeconds(1));

        // Throwing
        this._state = State.Shooting;
        await this.ThrowGrenade(worldPosition);

        // Cooloff
        this.ActionComplete();
    }

    private async Task ThrowGrenade(Vector3 targetPosition)
    {
        Transform grenadeTransform = Instantiate(this._grenadePrefab, transform.position, Quaternion.identity);
        GrenadeProjectileController grenadeProjectile = grenadeTransform.GetComponent<GrenadeProjectileController>();
        grenadeProjectile.ThrowGrenade(targetPosition, this.InvokeGrenadeAction);
        await Task.Delay(TimeSpan.FromMilliseconds(200));
    }
}
