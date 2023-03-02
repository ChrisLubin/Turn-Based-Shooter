using UnityEngine;

public class GlobalMouse : MonoBehaviour
{
    private static GlobalMouse _instance;

    private void Awake()
    {
        _instance = this;
    }

    public static bool TryGetIntersectingComponent<T>(int layerMaskId, out T component)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool didClickComponent = Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMaskId);

        if (didClickComponent && raycastHit.transform.TryGetComponent<T>(out T retrievedComponent))
        {
            component = retrievedComponent;
            return true;
        }

        component = default(T);
        return false;
    }

    public static bool IsIntersecting(int layerMaskId)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMaskId);
    }

    public static Vector3 GetFloorPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, (int) Constants.LayerMaskIds.MainFloor);
        return raycastHit.point;
    }
}
