using UnityEngine;

public class Soldier : MonoBehaviour
{
    private Vector3 _targetPosition;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown((int) Constants.MouseButtonIds.LeftClick))
        {
            this._targetPosition = GlobalMouse.GetFloorPosition();
        }
        HandleMove();
    }

    private void HandleMove()
    {
        if (Vector3.Distance(transform.position, this._targetPosition) < 0.05f)
        {
            return;
        }

        Vector3 moveDirection = (_targetPosition - transform.position).normalized;
        float moveSpeed = 4f;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
