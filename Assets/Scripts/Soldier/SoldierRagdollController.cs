using UnityEngine;

public class SoldierRagdollController : MonoBehaviour
{
    [SerializeField] private Transform _ragdollRootBone;

    public void DoRagroll(Transform originalRootBone)
    {
        this.MatchAllChildTransform(originalRootBone, this._ragdollRootBone);

        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        this.ApplyExplosionToRagdoll(this._ragdollRootBone, 400f, transform.position + randomDirection, 10f);
    }

    private void MatchAllChildTransform(Transform root, Transform clone)
    {
        foreach (Transform originalChild in root)
        {
            Transform cloneChild = clone.Find(originalChild.name);
            if (cloneChild != null)
            {
                cloneChild.position = originalChild.position;
                cloneChild.rotation = originalChild.rotation;

                MatchAllChildTransform(originalChild, cloneChild);
            }
        }
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
