using System;
using System.Threading.Tasks;
using UnityEngine;

public class SoldierShootActionController : BaseAction
{
    private State _state;
    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }
    public Action<Vector3, int> OnShoot;

    private void Awake()
    {
        this.MaxEffectiveDistance = 5;
        this.ActionCost = 1;
        this.TargetType = Constants.SoldierActionTargetTypes.Enemy;
    }

    private void Update()
    {
        // Debug.Log($"{this._state}");
    }

    protected async override void DoAction(Vector3 worldPosition)
    {
        this._state = State.Aiming;
        // Aiming
        Debug.Log($"Aiming");
        await Task.Delay(TimeSpan.FromSeconds(1));
        this._state = State.Shooting;

        // Shooting
        Debug.Log($"Shooting");
        this.OnShoot?.Invoke(worldPosition, 30);
        await Task.Delay(TimeSpan.FromSeconds(.1));
        this._state = State.Cooloff;

        // Cooloff
        Debug.Log($"Cooloff");
        await Task.Delay(TimeSpan.FromSeconds(.5));

        this._OnActionComplete();
    }

    public override string ToString() => Constants.SoldierActionNames.Shoot;
}
