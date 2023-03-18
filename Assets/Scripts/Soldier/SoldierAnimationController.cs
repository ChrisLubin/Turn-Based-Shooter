using UnityEngine;

public class SoldierAnimationController : MonoBehaviour
{
    private Animator _animator;
    private const string _IS_WALKING_ANIMATION_NAME = "IsWalking";
    private const string _IS_SHOOTING_ANIMATION_NAME = "IsShooting";
    private const string _DO_SWORD_SLASH_ANIMATION_NAME = "DoSwordSlash";
    public const int DO_SWORD_SLASH_TIME_MILLISECONDS = 1267;
    private const int _SHOOT_ANIMATION_DEFAULT_TIME_MILLISECONDS = 267;
    private const int _SHOOT_ANIMATION_SPEED_MULTIPLIER = 2;
    public const int SHOOT_ANIMATION_TIME_MILLISECONDS = _SHOOT_ANIMATION_DEFAULT_TIME_MILLISECONDS / _SHOOT_ANIMATION_SPEED_MULTIPLIER;

    private void Awake()
    {
        this._animator = GetComponentInChildren<Animator>();
        SoldierMoveActionController moveAction = GetComponent<SoldierMoveActionController>();
        SoldierShootActionController shootAction = GetComponent<SoldierShootActionController>();
        SoldierSwordActionController swordAction = GetComponent<SoldierSwordActionController>();
        moveAction.OnStartMoving += this.OnStartMoving;
        moveAction.OnStopMoving += this.OnStopMoving;
        shootAction.OnStartShoot += this.StartShoot;
        shootAction.OnStopShoot += this.StopShoot;
        swordAction.OnStartSwordSlash += this.DoSwordSlash;
    }

    private void OnStartMoving() => this._animator.SetBool(_IS_WALKING_ANIMATION_NAME, true);
    private void OnStopMoving() => this._animator.SetBool(_IS_WALKING_ANIMATION_NAME, false);
    public void StartShoot() => this._animator.SetBool(_IS_SHOOTING_ANIMATION_NAME, true);
    public void StopShoot() => this._animator.SetBool(_IS_SHOOTING_ANIMATION_NAME, false);
    public void DoSwordSlash() => this._animator.SetTrigger(_DO_SWORD_SLASH_ANIMATION_NAME);
}
