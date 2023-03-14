using Cinemachine;
using UnityEngine;

public class ActionCameraController : MonoBehaviour
{
    public static ActionCameraController Instance;
    private CinemachineVirtualCamera _camera;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            this._camera = GetComponent<CinemachineVirtualCamera>();
            return;
        }

        Debug.LogError("There's more than one ActionCameraController! " + transform + " - " + Instance);
        Destroy(gameObject);
    }

    private void Start()
    {
        this.SetActive(false);
        this._camera.Priority = 20;
    }

    private void SetActive(bool showCamera) => this.gameObject.SetActive(showCamera);
    public bool IsActive() => this.gameObject.activeSelf;
    public void StopShot() => this.SetActive(false);

    public void DoShot(Vector3 from, Vector3 to)
    {
        transform.position = from;
        transform.LookAt(to);
        this.SetActive(true);
    }

}
