using System;
using UnityEngine;

public class SoldierHealthController : MonoBehaviour
{
    [SerializeField] private int _health = 100;
    public Action OnDeath;

    public void TakeDamage(int damageAmount)
    {
        this._health -= damageAmount;
        this._health = this._health < 0 ? 0 : this._health;

        if (this._health == 0)
        {
            this.Die();
        }

        Debug.Log($"{this} took {damageAmount} damage!");
        Debug.Log($"Health is now {this._health}");
    }

    private void Die()
    {
        this.OnDeath?.Invoke();
    }
}
