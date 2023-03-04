using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void Update()
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
}
