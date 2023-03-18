using System;
using System.Threading.Tasks;
using UnityEngine;

public class SoldierShootActionController : BaseAction
{
    private State _state;
    private Vector3 _targetPosition;
    private enum State
    {
        None,
        Aiming,
        Shooting,
        Cooloff,
    }
    public Action<Vector3, int> OnShoot;
    public Action OnStartShoot;
    public Action OnStopShoot;
    [SerializeField] private Transform _bulletPrefab;
    [SerializeField] private Transform _shootPoint;
    private const int SHOOT_ANIMATION_TIME = SoldierAnimationController.SHOOT_ANIMATION_TIME_MILLISECONDS;

    private void Awake()
    {
        this.MaxEffectiveDistance = 6;
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
            Vector3 aimDirection = (this._targetPosition - transform.position).normalized;
            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
        }
    }

    public override string ToString() => Constants.SoldierActionNames.Shoot;
    private void InvokeShootAction() => this.OnShoot?.Invoke(this._targetPosition, 10);

    protected async override void DoAction(Vector3 worldPosition)
    {
        // Aiming
        this._state = State.Aiming;
        this._targetPosition = worldPosition;
        await Task.Delay(TimeSpan.FromSeconds(1));

        // Shooting
        this._state = State.Shooting;
        this.OnStartShoot?.Invoke();
        await this.ShootBullet(worldPosition);
        await this.ShootBullet(worldPosition);
        await this.ShootBullet(worldPosition);
        this.OnStopShoot?.Invoke();

        // Cooloff
        this._state = State.Cooloff;
        await Task.Delay(TimeSpan.FromSeconds(.5));
        this.ActionComplete();
    }

    private async Task ShootBullet(Vector3 targetPosition)
    {
        Transform bulletTransform = Instantiate(_bulletPrefab, _shootPoint.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletTransform.GetComponent<BulletProjectile>();
        bulletProjectile.SendBullet(targetPosition, this.InvokeShootAction);
        await Task.Delay(TimeSpan.FromMilliseconds(SHOOT_ANIMATION_TIME));
    }
}
