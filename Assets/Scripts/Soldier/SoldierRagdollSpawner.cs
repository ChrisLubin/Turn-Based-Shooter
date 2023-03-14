using UnityEngine;

public class SoldierRagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform _ragdollPrefab;
    [SerializeField] private Transform _originalRootBone;
    private SoldierHealthController _healthController;

    private void Awake()
    {
        this._healthController = GetComponent<SoldierHealthController>();
        this._healthController.OnDeath += this.OnDeath;
    }

    private void OnDeath()
    {
        Transform ragdollTransform = Instantiate(this._ragdollPrefab, transform.position, transform.rotation);
        SoldierRagdollController ragdollController = ragdollTransform.GetComponent<SoldierRagdollController>();
        ragdollController.DoRagroll(this._originalRootBone);
    }
}
