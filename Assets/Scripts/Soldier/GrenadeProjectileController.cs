using System;
using UnityEngine;

public class GrenadeProjectileController : MonoBehaviour
{
    [SerializeField] private Transform _grenadeExplodeVfxPrefab;
    [SerializeField] private AnimationCurve _arcYAnimationCurve;
    private TrailRenderer _trailRenderer;
    private Vector3 _targetPosition;
    public Action OnHit;
    private float _totalDistance;
    private Vector3 _positionXZ;

    private void Awake()
    {
        this._trailRenderer = GetComponentInChildren<TrailRenderer>();
    }

    private void Update()
    {
        Vector3 moveDirection = (this._targetPosition - _positionXZ).normalized;
        float moveSpeed = 15f;
        this._positionXZ += moveDirection * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(this._positionXZ, this._targetPosition);
        float distanceNormalized = 1 - distance / this._totalDistance;

        float maxHeight = this._totalDistance / 4f;
        float evaluatedPosition = this._arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        float positionY = !double.IsNaN(evaluatedPosition) ? evaluatedPosition : 0.4f;
        transform.position = new Vector3(this._positionXZ.x, positionY, this._positionXZ.z);

        float reachedTargetDistance = .2f;
        if (Vector3.Distance(this._positionXZ, this._targetPosition) < reachedTargetDistance)
        {
            float damageRadius = 4f;
            Collider[] colliders = Physics.OverlapSphere(this._targetPosition, damageRadius);

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<Soldier>(out Soldier soldier))
                {
                    soldier.TakeDamage(15);
                }
                else if (collider.TryGetComponent<Crate>(out Crate crate))
                {
                    crate.TakeDamage();
                }
            }

            this.OnHit();
            transform.position = this._targetPosition;
            this._trailRenderer.transform.parent = null;
            Instantiate(this._grenadeExplodeVfxPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public void ThrowGrenade(Vector3 targetPosition, Action OnHit)
    {
        this._positionXZ = transform.position;
        this._positionXZ.y = 0;
        this._totalDistance = Vector3.Distance(this._positionXZ, this._targetPosition);

        float distance = Vector3.Distance(this._positionXZ, this._targetPosition);
        float distanceNormalized = 1 - distance / this._totalDistance;

        float maxHeight = this._totalDistance / 4f;
        float evaluatedPosition = this._arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        float positionY = !double.IsNaN(evaluatedPosition) ? evaluatedPosition : 0.4f;

        this._targetPosition = new Vector3(targetPosition.x, positionY, targetPosition.z);
        transform.position += Vector3.up * positionY;
        this.OnHit = OnHit;
    }
}
