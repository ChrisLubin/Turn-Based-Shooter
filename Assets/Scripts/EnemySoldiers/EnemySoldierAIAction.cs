using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySoldierAIAction
{
    public string ActionName { get; private set; }
    private List<GridTile> _validGridTiles;
    private Soldier _soldier;

    public EnemySoldierAIAction(string actionName, GridTile[] validGridTiles, Soldier sodlier)
    {
        this.ActionName = actionName;
        this._validGridTiles = validGridTiles.ToList();
        this._soldier = sodlier;
    }

    public int GetBestActionValue()
    {
        if (this.ActionName == Constants.SoldierActionNames.Shoot)
        {
            return this.GetActionValue(LevelGrid.Instance.GetGridTile(this._soldier.transform.position));
        }
        if (this.ActionName == Constants.SoldierActionNames.Move)
        {
            int highestActionValue = 1;
            foreach (GridTile tile in this._validGridTiles)
            {
                int actionValue = this.GetActionValue(tile);
                if (actionValue > highestActionValue)
                {
                    highestActionValue = actionValue;
                }
            }

            return highestActionValue;
        }
        if (this.ActionName == Constants.SoldierActionNames.Spin)
        {
            return this.GetActionValue(LevelGrid.Instance.GetGridTile(this._soldier.transform.position));
        }

        return 0;
    }

    private int GetActionValue(GridTile gridTile)
    {
        if (this.ActionName == Constants.SoldierActionNames.Shoot)
        {
            int canShootAtCount = LevelGrid.Instance.GetValidGridTiles(Constants.SoldierActionTargetTypes.Enemy, this._soldier, LevelGrid.Instance.GetWorldPosition(gridTile), Constants.SoldierActionNames.Shoot).Count();

            if (canShootAtCount == 0)
            {
                return -1;
            }
            return 100 - (canShootAtCount - 1) * 2;
        }
        if (this.ActionName == Constants.SoldierActionNames.Move)
        {
            int canShootAtCount = LevelGrid.Instance.GetValidGridTiles(Constants.SoldierActionTargetTypes.Enemy, this._soldier, LevelGrid.Instance.GetWorldPosition(gridTile), Constants.SoldierActionNames.Shoot).Count();

            if (canShootAtCount == 0)
            {
                return 1;
            }
            return 99 - (canShootAtCount - 1) * 2;
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

        this._validGridTiles.Sort((GridTile a, GridTile b) =>
        {
            int distanceA = Mathf.Abs((int)this._soldier.transform.position.x - a.GetGridPosition().x) + Mathf.Abs((int)this._soldier.transform.position.z - a.GetGridPosition().z);
            int distanceB = Mathf.Abs((int)this._soldier.transform.position.x - b.GetGridPosition().x) + Mathf.Abs((int)this._soldier.transform.position.z - b.GetGridPosition().z);
            if (this.GetActionValue(a) == this.GetActionValue(b))
            {
                return distanceA - distanceB;
            }

            return this.GetActionValue(b) - this.GetActionValue(a);
        });

        GridTile highestActionValueGrid = this._validGridTiles[0];
        targetPosition = LevelGrid.Instance.GetWorldPosition(highestActionValueGrid);
        return true;
    }
}
