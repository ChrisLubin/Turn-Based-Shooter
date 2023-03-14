using System;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Vector3 _targetPosition;
    private TrailRenderer _trailRenderer;
    [SerializeField] private Transform _bulletHitVfxPrefab;
    public Action OnHit;

    private void Awake()
    {
        this._trailRenderer = GetComponentInChildren<TrailRenderer>();
    }

    private void Update()
    {
        float distanceBeforeMoving = Vector3.Distance(transform.position, this._targetPosition);
        float moveSpeed = 200f;
        Vector3 moveDirection = (this._targetPosition - transform.position).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        float distanceAfterMoving = Vector3.Distance(transform.position, this._targetPosition);

        if (distanceBeforeMoving < distanceAfterMoving)
        {
            this.OnHit();
            transform.position = this._targetPosition;
            this._trailRenderer.transform.parent = null;
            Destroy(this.gameObject);
            Instantiate(this._bulletHitVfxPrefab, this._targetPosition, Quaternion.identity);
        }
    }

    public void SendBullet(Vector3 targetPosition, Action OnHit)
    {
        this._targetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        this.OnHit = OnHit;
    }
}
