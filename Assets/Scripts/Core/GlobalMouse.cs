using System;
using UnityEngine;

public class GlobalMouse : MonoBehaviour
{
    public static GlobalMouse Instance;
    private readonly int _LEFT_CLICK_ID = 0;
    private readonly int _RIGHT_CLICK_ID = 1;
    public event Action<int, GameObject> OnLayerLeftClick;
    public event Action OnRightClick;

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
        if (Input.GetMouseButtonDown(this._RIGHT_CLICK_ID))
        {
            this.OnRightClick?.Invoke();
            return;
        }
        else if (Input.GetMouseButtonDown(this._LEFT_CLICK_ID))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue);

            if (raycastHit.collider == null)
            {
                return;
            }

            GameObject gameObject = raycastHit.collider.gameObject;
            int layerMaskId = 1 << gameObject.layer;

            if (this.ShouldPassThroughClick(layerMaskId))
            {
                this.OnLayerLeftClick?.Invoke(layerMaskId, gameObject);
            }
            return;
        }
    }

    private bool ShouldPassThroughClick(int layerMaskId)
    {
        if (layerMaskId == (int)Constants.LayerMaskIds.Default)
        {
            return false;
        }
        if (layerMaskId == (int)Constants.LayerMaskIds.IgnoreRaycast)
        {
            return false;
        }
        if (layerMaskId == (int)Constants.LayerMaskIds.Obstacle)
        {
            return false;
        }

        return true;
    }

    public Vector3 GetFloorPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, (int)Constants.LayerMaskIds.MainFloor);
        return raycastHit.point;
    }
}
