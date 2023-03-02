using UnityEngine;

public class GridController
{
    private int _width;
    private int _height;
    private float _cellSize;

    public GridController(int width, int height, float cellSize)
    {
        this._width = width;
        this._height = height;
        this._cellSize = cellSize;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z) + Vector3.right * .2f, Color.white, 1000);
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * this._cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition((int) (worldPosition.x / _cellSize), (int) (worldPosition.z / _cellSize));
    }
}
