using UnityEngine;

public class Soldier : MonoBehaviour
{
    private Vector3 _targetPosition;
    private Animator _soldierAnimator;
    private GridPosition _gridPosition;
    private const string IS_WALKING_ANIMATION_NAME = "IsWalking";

    private void Awake()
    {
        this._soldierAnimator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        this._gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetSoldierAtGridPosition(this._gridPosition, this);
        SetTargetPosition(transform.position);
    }

    private void Update()
    {
        HandleMove();
        HandleGridPosition();
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        this._targetPosition = targetPosition;
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

    private void HandleGridPosition()
    {
        GridPosition currentGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        Soldier soldierAtGridPosition = LevelGrid.Instance.GetSoldierAtGridPosition(currentGridPosition);
        if (this._gridPosition != currentGridPosition || this != soldierAtGridPosition)
        {
            LevelGrid.Instance.OnSoldierMovedGridPosition(this, this._gridPosition, currentGridPosition);
        }
    }

    public void SetGridPosition(GridPosition gridPosition)
    {
        this._gridPosition = gridPosition;
    }
}
