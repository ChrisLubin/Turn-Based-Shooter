using UnityEngine;

public class GlobalMouse : MonoBehaviour
{
    private static GlobalMouse _instance;

    private void Awake()
    {
        _instance = this;
    }

    public static Vector3 GetFloorPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, (int) Constants.LayerMaskIds.MainFloor);
        return raycastHit.point;
    }
}
