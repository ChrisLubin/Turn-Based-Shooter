using UnityEngine;

public class Soldier : MonoBehaviour
{
    private Vector3 _targetPosition;
    private Animator _soldierAnimator;
    private const string IS_WALKING_ANIMATION_NAME = "IsWalking";

    private void Start()
    {
        this._soldierAnimator = GetComponentInChildren<Animator>();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown((int) Constants.MouseButtonIds.LeftClick))
        {
            this._targetPosition = GlobalMouse.GetFloorPosition();
        }
        HandleMove();
    }

    private void HandleMove()
    {
        if (Vector3.Distance(transform.position, this._targetPosition) < 0.05f)
        {
            _soldierAnimator.SetBool(IS_WALKING_ANIMATION_NAME, false);
            return;
        }

        _soldierAnimator.SetBool(IS_WALKING_ANIMATION_NAME, true);
        float moveSpeed = 4f;
        float rotateSpeed = 10f;
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
