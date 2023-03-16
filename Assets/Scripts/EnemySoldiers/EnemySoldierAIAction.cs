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

    public int GetBestActionValue()
    {
        if (this.ActionName == Constants.SoldierActionNames.Shoot)
        {
            return 100;
        }
        if (this.ActionName == Constants.SoldierActionNames.Move)
        {
            return 10 * LevelGrid.Instance.GetValidGridTiles(Constants.SoldierActionTargetTypes.Enemy, this._soldier, this._soldier.transform.position, Constants.SoldierActionNames.Shoot).Count();
        }
        if (this.ActionName == Constants.SoldierActionNames.Spin)
        {
            return 0;
        }

        return 0;
    }

    private int GetActionValue(GridTile gridTile)
    {
        if (this.ActionName == Constants.SoldierActionNames.Shoot)
        {
            return 100;
        }
        if (this.ActionName == Constants.SoldierActionNames.Move)
        {
            return 10 * LevelGrid.Instance.GetValidGridTiles(Constants.SoldierActionTargetTypes.Enemy, this._soldier, LevelGrid.Instance.GetWorldPosition(gridTile), Constants.SoldierActionNames.Shoot).Count();
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

        GridTile highestActionValueGrid = this._validGridTiles[0];

        foreach (GridTile gridTile in this._validGridTiles)
        {
            if (this.GetActionValue(gridTile) > this.GetActionValue(highestActionValueGrid))
            {
                highestActionValueGrid = gridTile;
            }
        }

        targetPosition = LevelGrid.Instance.GetWorldPosition(highestActionValueGrid);
        return true;
    }
}
