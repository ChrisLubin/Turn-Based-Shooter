using UnityEngine;
using UnityEngine.AI;

public class Floor : MonoBehaviour
{
    public static Floor Instance { get; private set; }
    private NavMeshSurface _navMeshSurface;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            this._navMeshSurface = GetComponent<NavMeshSurface>();
            this.RebakeFloor();
            return;
        }

        Debug.LogError("There's more than one Floor! " + transform + " - " + Instance);
        Destroy(gameObject);
    }

    public void RebakeFloor()
    {
        this._navMeshSurface.BuildNavMesh();
    }
}
