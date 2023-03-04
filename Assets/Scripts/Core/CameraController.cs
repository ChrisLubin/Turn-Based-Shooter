using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineTransposer _cinemachineTransposer;
    private Vector3 _targetFollowOffset;
    private const float _MIN_FOLLOW_Y_OFFSET = 2f;
    private const float _MAX_FOLLOW_Y_OFFSET = 12f;

    private void Awake()
    {
        CinemachineVirtualCamera cinemachineVirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        this._cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Start()
    {
        this._targetFollowOffset = this._cinemachineTransposer.m_FollowOffset;
    }

    private void Update()
    {
        this.HandleMovement();
        this.HandlePan();
        this.HandleTilt();
    }

    private void HandleMovement()
    {
        Vector3 inputMoveDir = new();
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x += 1;
        }

        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveVector * 10f * Time.deltaTime;
    }

    private void HandlePan()
    {
        Vector3 rotationVector = new();
        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y += 1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y -= 1f;
        }
        transform.eulerAngles += rotationVector * 100f * Time.deltaTime;
    }

    private void HandleTilt()
    {
        float tiltAmount = 1f;
        float tiltSpeed = 5f;
        this._cinemachineTransposer.m_FollowOffset = Vector3.Lerp(this._cinemachineTransposer.m_FollowOffset, this._targetFollowOffset, Time.deltaTime * tiltSpeed);

        if (Input.mouseScrollDelta.y == 0)
        {
            return;
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            this._targetFollowOffset.y += tiltAmount;
        }
        else
        {
            this._targetFollowOffset.y -= tiltAmount;
        }

        this._targetFollowOffset.y = Mathf.Clamp(this._targetFollowOffset.y, _MIN_FOLLOW_Y_OFFSET, _MAX_FOLLOW_Y_OFFSET);
    }
}
