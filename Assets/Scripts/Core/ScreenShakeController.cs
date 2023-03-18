using Cinemachine;
using UnityEngine;

public class ScreenShakeController : MonoBehaviour
{
    public static ScreenShakeController Instance { get; private set; }
    private CinemachineImpulseSource _cinemachineImpulseSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            this._cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
            return;
        }

        Debug.LogError("There's more than one ScreenShakeController! " + transform + " - " + Instance);
        Destroy(gameObject);
    }

    public void Shake(float intensity = 0.3f)
    {
        this._cinemachineImpulseSource.GenerateImpulse(intensity);
    }
}
