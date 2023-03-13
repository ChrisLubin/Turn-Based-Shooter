using System;
using UnityEngine;

public class SoldierMoveActionController : BaseAction
{
    private Vector3 _targetPosition;
    public Action OnStartMoving;
    public Action OnStopMoving;

    private void Awake()
    {
        this.MaxEffectiveDistance = 2;
        this.ActionCost = 1;
        this.TargetType = Constants.SoldierActionTargetTypes.EmptyTile;
    }

    private void Start()
    {
        this.SetTargetPosition(transform.position);
    }

    private void Update()
    {
        if (!this.IsActive)
        {
            return;
        }

        this.HandleMove();
    }

    private void SetTargetPosition(Vector3 targetPosition) => this._targetPosition = targetPosition;
    public override string ToString() => Constants.SoldierActionNames.Move;

    protected override void DoAction(Vector3 targetPosition)
    {
        this.OnStartMoving?.Invoke();
        this.SetTargetPosition(targetPosition);
    }

    private void HandleMove()
    {
        if (Vector3.Distance(transform.position, this._targetPosition) < 0.05f)
        {
            this.OnStopMoving?.Invoke();
            this.ActionComplete();
            return;
        }

        float moveSpeed = 4f;
        float rotateSpeed = 10f;
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
