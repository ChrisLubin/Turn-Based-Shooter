using UnityEngine;

public class SoldierAnimationController : MonoBehaviour
{
    private Animator _animator;
    private SoldierMoveActionController _moveAction;
    private SoldierShootActionController _shootAction;
    private const string _IS_WALKING_ANIMATION_NAME = "IsWalking";
    private const string _IS_SHOOTING_ANIMATION_NAME = "IsShooting";
    private const int _SHOOT_ANIMATION_DEFAULT_TIME_MILLISECONDS = 267;
    private const int _SHOOT_ANIMATION_SPEED_MULTIPLIER = 2;
    public const int SHOOT_ANIMATION_TIME_MILLISECONDS = _SHOOT_ANIMATION_DEFAULT_TIME_MILLISECONDS / _SHOOT_ANIMATION_SPEED_MULTIPLIER;

    private void Awake()
    {
        this._animator = GetComponentInChildren<Animator>();
        this._moveAction = GetComponent<SoldierMoveActionController>();
        this._shootAction = GetComponent<SoldierShootActionController>();
        this._moveAction.OnStartMoving += this.OnStartMoving;
        this._moveAction.OnStopMoving += this.OnStopMoving;
        this._shootAction.OnStartShoot += this.StartShoot;
        this._shootAction.OnStopShoot += this.StopShoot;
    }

    private void OnStartMoving() => this._animator.SetBool(_IS_WALKING_ANIMATION_NAME, true);
    private void OnStopMoving() => this._animator.SetBool(_IS_WALKING_ANIMATION_NAME, false);
    public void StartShoot() => this._animator.SetBool(_IS_SHOOTING_ANIMATION_NAME, true);
    public void StopShoot() => this._animator.SetBool(_IS_SHOOTING_ANIMATION_NAME, false);
}
