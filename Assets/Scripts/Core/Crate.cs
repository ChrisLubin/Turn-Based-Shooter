using UnityEngine;

public class Crate : MonoBehaviour
{
    private void OnDestroy()
    {
        Floor.Instance.RebakeFloor();
    }

    public void TakeDamage()
    {
        Destroy(gameObject);
    }
}
