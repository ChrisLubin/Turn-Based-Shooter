using System;
using System.Threading.Tasks;
using UnityEngine;

public class SoldierSwordActionController : BaseAction
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
    public Action OnStartSwordSlash;
    public Action<Vector3, int> OnHit;
    private const int SWORD_ANIMATION_TIME = SoldierAnimationController.DO_SWORD_SLASH_TIME_MILLISECONDS;
    [SerializeField] private GameObject _rifleObject;
    [SerializeField] private GameObject _swordObject;

    private void Awake()
    {
        this.MaxEffectiveDistance = 1;
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

    public override string ToString() => Constants.SoldierActionNames.Sword;

    protected async override void DoAction(Vector3 worldPosition)
    {
        // Aiming
        this._state = State.Aiming;
        this._targetPosition = worldPosition;
        await Task.Delay(TimeSpan.FromSeconds(1));

        // Shooting
        this._state = State.Shooting;
        await this.DoSword();

        // Cooloff
        this._state = State.Cooloff;
        await Task.Delay(TimeSpan.FromSeconds(.5));
        this.ActionComplete();
    }

    private async Task DoSword()
    {
        this._rifleObject.SetActive(false);
        this._swordObject.SetActive(true);
        this.OnStartSwordSlash?.Invoke();
        await Task.Delay(TimeSpan.FromMilliseconds(SWORD_ANIMATION_TIME / 2));
        this.OnHit?.Invoke(this._targetPosition, 100);
        ScreenShakeController.Instance.Shake(.7f);
        await Task.Delay(TimeSpan.FromMilliseconds(SWORD_ANIMATION_TIME / 2));
        this._swordObject.SetActive(false);
        this._rifleObject.SetActive(true);
    }
}
