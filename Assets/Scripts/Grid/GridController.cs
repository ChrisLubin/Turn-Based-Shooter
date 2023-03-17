using System.Collections.Generic;
using UnityEngine;

public interface ITGridTile { public GridPosition GetGridPosition(); public void SetGridPosition(GridPosition gridPosition); }

public class GridController<TGridTile> where TGridTile : ITGridTile
{
    private int _width;
    private int _height;
    private float _cellSize;
    private TGridTile[,] _gridTileMatrix;

    public GridController(int width, int height, float cellSize)
    {
        this._width = width;
        this._height = height;
        this._cellSize = cellSize;
        this._gridTileMatrix = new TGridTile[width, height];
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition) => new Vector3(gridPosition.x, 0, gridPosition.z) * this._cellSize;
    public GridPosition GetGridPosition(Vector3 worldPosition) => new(Mathf.RoundToInt(worldPosition.x / this._cellSize), Mathf.RoundToInt(worldPosition.z / this._cellSize));
    public bool IsValidGridPosition(GridPosition gridPosition) => gridPosition.x >= 0 && gridPosition.z >= 0 && gridPosition.x < this._width && gridPosition.z < this._height;

    public TGridTile GetGridTile(Vector3 worldPosition)
    {
        GridPosition gridPosition = GetGridPosition(worldPosition);
        return this._gridTileMatrix[gridPosition.x, gridPosition.z];
    }

    public void CreateGridTiles(Transform parentTransform, Transform gridTilePrefab)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new(x, z);
                Transform debugTransform = GameObject.Instantiate(gridTilePrefab, GetWorldPosition(gridPosition), Quaternion.identity, parentTransform);
                TGridTile gridTile = debugTransform.GetComponent<TGridTile>();
                gridTile.SetGridPosition(gridPosition);
                _gridTileMatrix[x, z] = gridTile;
            }
        }
    }

    public TGridTile[] GetSurroundingGridTiles(TGridTile originalGridTile, int offset, bool doDiamondShape = false)
    {
        List<TGridTile> surroundingGridTiles = new();
        GridPosition originalGridPosition = originalGridTile.GetGridPosition();

        for (int x = originalGridPosition.x - offset; x <= originalGridPosition.x + offset; x++)
        {
            for (int z = originalGridPosition.z - offset; z <= originalGridPosition.z + offset; z++)
            {
                int distanceToGridPosition = Mathf.Abs(x - originalGridPosition.x) + Mathf.Abs(z - originalGridPosition.z);

                if (doDiamondShape && distanceToGridPosition > offset)
                {
                    continue;
                }

                GridPosition gridPosition = new(x, z);
                if (IsValidGridPosition(gridPosition) && originalGridPosition != gridPosition)
                {
                    TGridTile gridTile = this._gridTileMatrix[x, z];
                    surroundingGridTiles.Add(gridTile);
                }
            }
        }

        return surroundingGridTiles.ToArray();
    }
}
