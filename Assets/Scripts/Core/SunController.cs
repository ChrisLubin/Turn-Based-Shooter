using UnityEngine;

public class SunController : MonoBehaviour
{
    [SerializeField] private float _speedUp;
    [SerializeField] private float _speedDown;

    void Update()
    {
        if (transform.eulerAngles.x < 360f && transform.eulerAngles.x < 270f)
        {
            transform.localRotation *= Quaternion.Euler(-this._speedUp, 0, 0);
        }
        else
        {
            transform.localRotation *= Quaternion.Euler(-this._speedDown, 0, 0);
        }
    }
}
