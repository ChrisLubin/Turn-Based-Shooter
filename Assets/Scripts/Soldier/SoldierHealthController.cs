using System;
using UnityEngine;

public class SoldierHealthController : MonoBehaviour
{
    [SerializeField] private int _health = 100;
    public Action<int> OnHealthChange;
    public Action OnDeath;

    private void Die() => this.OnDeath?.Invoke();
    public int GetHealth() => this._health;

    public void TakeDamage(int damageAmount)
    {
        this._health -= damageAmount;
        this._health = this._health < 0 ? 0 : this._health;
        this.OnHealthChange?.Invoke(this._health);

        if (this._health == 0)
        {
            this.Die();
        }
    }

}
