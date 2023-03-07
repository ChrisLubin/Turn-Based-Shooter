using System.Collections.Generic;
using UnityEngine;

public class GridController
{
    private int _width;
    private int _height;
    private float _cellSize;
    private GridTile[,] _gridTileMatrix;

    public GridController(int width, int height, float cellSize)
    {
        this._width = width;
        this._height = height;
        this._cellSize = cellSize;
        this._gridTileMatrix = new GridTile[width, height];
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition) => new Vector3(gridPosition.x, 0, gridPosition.z) * this._cellSize;
    public GridPosition GetGridPosition(Vector3 worldPosition) => new GridPosition(Mathf.RoundToInt(worldPosition.x / this._cellSize), Mathf.RoundToInt(worldPosition.z / this._cellSize));
    public GridTileSoldierController GetGridTileSoldierController(GridPosition gridPosition) => this._gridTileMatrix[gridPosition.x, gridPosition.z].GetGridTileSoldierController();
    public bool IsValidGridPosition(GridPosition gridPosition) => gridPosition.x >= 0 && gridPosition.z >= 0 && gridPosition.x < this._width && gridPosition.z < this._height;

    public GridTile GetGridTile(Vector3 worldPosition)
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
                GridTile gridTile = debugTransform.GetComponent<GridTile>();
                gridTile.SetGridPosition(gridPosition);
                _gridTileMatrix[x, z] = gridTile;
            }
        }
    }

    public GridTile[] GetSurroundingGridTiles(GridTile originalGridTile, int offset)
    {
        List<GridTile> surroundingGridTiles = new();
        GridPosition originalGridPosition = originalGridTile.GetGridPosition();

        for (int x = originalGridPosition.x - offset; x <= originalGridPosition.x + offset; x++)
        {
            for (int z = originalGridPosition.z - offset; z <= originalGridPosition.z + offset; z++)
            {
                GridPosition gridPosition = new(x, z);
                if (IsValidGridPosition(gridPosition) && originalGridPosition != gridPosition)
                {
                    GridTile gridTile = this._gridTileMatrix[x, z];
                    surroundingGridTiles.Add(gridTile);
                }
            }
        }

        return surroundingGridTiles.ToArray();
    }
}
