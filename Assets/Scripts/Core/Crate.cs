using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField] private Transform _destroyedCreatePrefab;

    private void OnDestroy()
    {
        Floor.Instance.RebakeFloor();
    }

    public void TakeDamage()
    {
        Transform destroyedCrate = Instantiate(this._destroyedCreatePrefab, transform.position, transform.rotation);
        this.ApplyExplosionToRagdoll(destroyedCrate, 150f, transform.position, 10f);
        Destroy(gameObject);
    }

    private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidBody))
            {
                childRigidBody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            this.ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
