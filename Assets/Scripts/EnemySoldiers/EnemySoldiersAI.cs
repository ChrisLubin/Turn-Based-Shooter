using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class EnemySoldiersAI : MonoBehaviour
{
    private void Start()
    {
        TurnController.Instance.OnTurnEnd += this.OnTurnEnd;
    }

    private async void OnTurnEnd(bool isPlayerTurn)
    {
        if (isPlayerTurn)
        {
            return;
        }

        Soldier[] aiSoldiers = GameObject.FindObjectsOfType<Soldier>().Where(soldier => soldier.GetIsEnemy()).ToArray();

        foreach (Soldier soldier in aiSoldiers)
        {
            while (soldier.CanDoAnyAction())
            {
                string[] actionNames = soldier.GetActionNames();
                List<string> actionNamesHavePointsFor = new();

                foreach (string actionName in actionNames)
                {
                    if (soldier.CanDoAction(actionName))
                    {
                        actionNamesHavePointsFor.Add(actionName);
                    }
                }

                List<string> actionNamesWithValidGridPositions = new();
                List<EnemySoldierAIAction> aiActions = new();

                foreach (string actionName in actionNamesHavePointsFor)
                {
                    GridTile[] validGridTiles = LevelGrid.Instance.GetValidGridTiles(soldier.GetActionTargetType(actionName), soldier, actionName);
                    if (validGridTiles.Count() > 0)
                    {
                        EnemySoldierAIAction action = new(actionName, validGridTiles, LevelGrid.Instance.GetGridTile(soldier.transform.position), soldier);
                        aiActions.Add(action);
                        actionNamesWithValidGridPositions.Add(actionName);
                    }
                }

                aiActions.Sort((EnemySoldierAIAction a, EnemySoldierAIAction b) => b.GetActionValue() - a.GetActionValue());

                if (aiActions.Count > 0)
                {
                    EnemySoldierAIAction bestAction = aiActions[0];

                    if (bestAction.TryGetTargetPosition(out Vector3 worldPosition))
                    {
                        await soldier.DoAction(worldPosition, bestAction.ActionName);
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }
                }

            }
        }

        TurnController.Instance.TriggerNextTurn();
    }
}
