using UnityEngine;

public class SoldierMoveActionController : MonoBehaviour
{
    private Vector3 _targetPosition;
    private Animator _soldierAnimator;
    private const string _IS_WALKING_ANIMATION_NAME = "IsWalking";

    private void Awake()
    {
        this._soldierAnimator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        this.SetTargetPosition(transform.position);
    }

    private void Update()
    {
        this.HandleMove();
    }

    public void SetTargetPosition(Vector3 targetPosition) => this._targetPosition = targetPosition;

    private void HandleMove()
    {
        if (Vector3.Distance(transform.position, this._targetPosition) < 0.05f)
        {
            this._soldierAnimator.SetBool(_IS_WALKING_ANIMATION_NAME, false);
            return;
        }

        this._soldierAnimator.SetBool(_IS_WALKING_ANIMATION_NAME, true);
        float moveSpeed = 4f;
        float rotateSpeed = 10f;
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
