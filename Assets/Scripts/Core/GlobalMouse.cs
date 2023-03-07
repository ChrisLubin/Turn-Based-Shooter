using System;
using UnityEngine;

public class GlobalMouse : MonoBehaviour
{
    public static GlobalMouse Instance;
    private readonly int _LEFT_CLICK_ID = 0;
    private readonly int _RIGHT_CLICK_ID = 1;
    public event Action<int, GameObject> OnLayerLeftClick;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }

        Debug.LogError("There's more than one GlobalMouse! " + transform + " - " + Instance);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(this._LEFT_CLICK_ID))
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue);
        GameObject gameObject = raycastHit.collider.gameObject;
        int layerMaskId = gameObject.layer;
        if (layerMaskId != (int)Constants.LayerMaskIds.Default && layerMaskId != (int)Constants.LayerMaskIds.IgnoreRaycast)
        {
            this.OnLayerLeftClick?.Invoke(1 << layerMaskId, gameObject);
        }
    }

    public Vector3 GetFloorPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, (int)Constants.LayerMaskIds.MainFloor);
        return raycastHit.point;
    }
}
