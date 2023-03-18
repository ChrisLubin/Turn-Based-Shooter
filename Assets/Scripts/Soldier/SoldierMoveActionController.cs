using System;
using UnityEngine;
using UnityEngine.AI;

public class SoldierMoveActionController : BaseAction
{
    public Action OnStartMoving;
    public Action OnStopMoving;
    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        this._navMeshAgent = GetComponent<NavMeshAgent>();
        this.MaxEffectiveDistance = 4;
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

    private void SetTargetPosition(Vector3 targetPosition) => this._navMeshAgent.destination = targetPosition;
    public override string ToString() => Constants.SoldierActionNames.Move;

    protected override void DoAction(Vector3 targetPosition)
    {
        this.OnStartMoving?.Invoke();
        this.SetTargetPosition(targetPosition);
    }

    private void HandleMove()
    {
        if (Vector3.Distance(transform.position, this._navMeshAgent.destination) < 0.05f)
        {
            this.OnStopMoving?.Invoke();
            this.ActionComplete();
            return;
        }
    }
}
