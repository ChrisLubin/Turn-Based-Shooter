using System;
using UnityEngine;

public class GrenadeProjectileController : MonoBehaviour
{
    private Vector3 _targetPosition;
    public Action OnHit;

    private void Update()
    {
        Vector3 moveDirection = (this._targetPosition - transform.position).normalized;
        float moveSpeed = 15f;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        float reachedTargetDistance = .2f;
        if (Vector3.Distance(transform.position, this._targetPosition) < reachedTargetDistance)
        {
            float damageRadius = 4f;
            Collider[] colliders = Physics.OverlapSphere(this._targetPosition, damageRadius);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<Soldier>(out Soldier soldier))
                {
                    soldier.TakeDamage(30);
                }
            }

            this.OnHit();
            transform.position = this._targetPosition;
            Destroy(this.gameObject);
        }
    }

    public void ThrowGrenade(Vector3 targetPosition, Action OnHit)
    {
        this._targetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        this.OnHit = OnHit;
    }
}
