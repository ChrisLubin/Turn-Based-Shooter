using System;
using System.Threading.Tasks;
using UnityEngine;

public class SoldierInteractActionController : BaseAction
{
    public Action<Vector3> OnInteract;

    private void Awake()
    {
        this.MaxEffectiveDistance = 1;
        this.ActionCost = 0;
        this.TargetType = Constants.SoldierActionTargetTypes.Door;
    }

    private void Update()
    {
        if (!this.IsActive)
        {
            return;
        }

    }

    public override string ToString() => Constants.SoldierActionNames.Interact;

    protected async override void DoAction(Vector3 targetPosition)
    {
        this.OnInteract?.Invoke(targetPosition);
        await Task.Delay(TimeSpan.FromSeconds(1));
        Floor.Instance.RebakeFloor();
        this.ActionComplete();
    }
}
