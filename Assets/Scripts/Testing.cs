using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Transform _gridDebugObject;
    private GridController _gridController;

    private void Awake()
    {
        this._gridController = new GridController(4, 4, 2f);
    }

    private void Start()
    {
        this._gridController.CreateDebugObjects(this.transform, _gridDebugObject);
    }
}
