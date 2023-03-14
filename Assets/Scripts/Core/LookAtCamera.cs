using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform _cameraTransform;

    private void Awake()
    {
        this._cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        Vector3 dirToCamera = (this._cameraTransform.position - transform.position).normalized;
        transform.LookAt(transform.position + dirToCamera * -1);
    }
}
