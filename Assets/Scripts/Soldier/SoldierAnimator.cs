using UnityEngine;

public class SoldierAnimator : MonoBehaviour
{
    private Animator _animator;
    private SoldierMoveActionController _moveAction;
    private SoldierShootActionController _shootAction;
    private const string _IS_WALKING_ANIMATION_NAME = "IsWalking";
    private const string _SHOOT_ANIMATION_NAME = "Shoot";

    private void Awake()
    {
        this._animator = GetComponentInChildren<Animator>();
        this._shootAction = GetComponent<SoldierShootActionController>();
        this._shootAction.OnShoot += this.OnShoot;
        this._moveAction = GetComponent<SoldierMoveActionController>();
        this._moveAction.OnStartMoving += this.OnStartMoving;
        this._moveAction.OnStopMoving += this.OnStopMoving;
    }

    private void OnStartMoving() => this._animator.SetBool(_IS_WALKING_ANIMATION_NAME, true);
    private void OnStopMoving() => this._animator.SetBool(_IS_WALKING_ANIMATION_NAME, false);
    private void OnShoot(Vector3 _, int __) => this._animator.SetTrigger(_SHOOT_ANIMATION_NAME);
}
