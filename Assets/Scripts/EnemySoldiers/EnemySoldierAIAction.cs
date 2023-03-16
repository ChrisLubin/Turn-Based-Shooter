using System.Linq;
using UnityEngine;

public class EnemySoldierAIAction
{
    public string ActionName { get; private set; }
    private GridTile[] _validGridTiles;
    private GridTile _originalGridTile;
    private Soldier _soldier;

    public EnemySoldierAIAction(string actionName, GridTile[] validGridTiles, GridTile originalGridTile, Soldier sodlier)
    {
        this.ActionName = actionName;
        this._validGridTiles = validGridTiles;
        this._originalGridTile = originalGridTile;
        this._soldier = sodlier;
    }

    public int GetActionValue()
    {
        if (this.ActionName == Constants.SoldierActionNames.Shoot)
        {
            return 100;
        }
        if (this.ActionName == Constants.SoldierActionNames.Move)
        {
            return 10 & LevelGrid.Instance.GetValidGridTiles(Constants.SoldierActionTargetTypes.Enemy, this._soldier, Constants.SoldierActionNames.Shoot).Count();
        }
        if (this.ActionName == Constants.SoldierActionNames.Spin)
        {
            return 0;
        }

        return 0;
    }

    public bool TryGetTargetPosition(out Vector3 targetPosition)
    {
        if (this._validGridTiles.Count() == 0)
        {
            targetPosition = new Vector3();
            return false;
        }

        GridTile closestGridTile = this._validGridTiles[0];

        foreach (GridTile gridTile in this._validGridTiles)
        {
            int distanceToGridTile = Mathf.Abs(gridTile.GetGridPosition().x - this._originalGridTile.GetGridPosition().x) + Mathf.Abs(gridTile.GetGridPosition().z - this._originalGridTile.GetGridPosition().z);
            int distanceToCurrentClosestGridPosition = Mathf.Abs(closestGridTile.GetGridPosition().x - this._originalGridTile.GetGridPosition().x) + Mathf.Abs(closestGridTile.GetGridPosition().z - this._originalGridTile.GetGridPosition().z);

            if (distanceToGridTile < distanceToCurrentClosestGridPosition)
            {
                closestGridTile = gridTile;
            }
        }

        targetPosition = LevelGrid.Instance.GetWorldPosition(closestGridTile);
        return true;
    }
}
