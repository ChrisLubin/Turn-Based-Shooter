using Cinemachine;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    private CinemachineTransposer _cinemachineTransposer;
    private Vector3 _targetFollowOffset;
    private Vector3 _targetMovementPosition;
    private Vector3 _targetPanPosition;
    private const float _MIN_FOLLOW_Y_OFFSET = 2f;
    private const float _MAX_FOLLOW_Y_OFFSET = 12f;
    [SerializeField] private float _moveSmoothingSpeed;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _panSmoothingSpeed;
    [SerializeField] private float _panSpeed;
    [SerializeField] private float _tiltSmoothingSpeed;
    [SerializeField] private float _tiltSpeed;

    private void Awake()
    {
        CinemachineVirtualCamera cinemachineVirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        this._cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Start()
    {
        this._targetMovementPosition = transform.position;
        this._targetPanPosition = transform.eulerAngles;
        this._targetFollowOffset = this._cinemachineTransposer.m_FollowOffset;
        this._moveSmoothingSpeed = 6f;
        this._moveSpeed = 16f;
        this._panSmoothingSpeed = 5.91f;
        this._panSpeed = 120f;
        this._tiltSmoothingSpeed = 5f;
        this._tiltSpeed = 60f;
    }

    private void Update()
    {
        if (ActionCameraController.Instance.IsActive())
        {
            return;
        }

        this.HandleMovement();
        this.HandlePan();
        this.HandleTilt();
    }

    private void HandleMovement()
    {
        Vector3 inputMoveDir = new();
        transform.position = Vector3.Lerp(transform.position, this._targetMovementPosition, Time.deltaTime * this._moveSmoothingSpeed);
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z += this._moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z -= this._moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x -= this._moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x += this._moveSpeed * Time.deltaTime;
        }

        this._targetMovementPosition += transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
    }

    private void HandlePan()
    {
        float eulerYAngleWithNegatives = transform.eulerAngles.y > 180 ? transform.eulerAngles.y - 360 : transform.eulerAngles.y;
        Vector3 eulerAnglesWithNegatives = new(0, eulerYAngleWithNegatives, 0);
        transform.eulerAngles = Vector3.Lerp(eulerAnglesWithNegatives, this._targetPanPosition, Time.deltaTime * this._panSmoothingSpeed);

        if (Input.GetKey(KeyCode.Q))
        {
            this._targetPanPosition.y += this._panSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            this._targetPanPosition.y -= this._panSpeed * Time.deltaTime;
        }

        this._targetPanPosition.y = Mathf.Clamp(this._targetPanPosition.y, -180f, 180f);
    }

    private void HandleTilt()
    {
        this._cinemachineTransposer.m_FollowOffset = Vector3.Lerp(this._cinemachineTransposer.m_FollowOffset, this._targetFollowOffset, Time.deltaTime * this._tiltSmoothingSpeed);

        if (Input.mouseScrollDelta.y == 0)
        {
            return;
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            this._targetFollowOffset.y += this._tiltSpeed * Time.deltaTime;
        }
        else
        {
            this._targetFollowOffset.y -= this._tiltSpeed * Time.deltaTime;
        }

        this._targetFollowOffset.y = Mathf.Clamp(this._targetFollowOffset.y, _MIN_FOLLOW_Y_OFFSET, _MAX_FOLLOW_Y_OFFSET);
    }
}
