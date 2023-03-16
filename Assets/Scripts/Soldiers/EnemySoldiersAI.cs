using System;
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
        await Task.Delay(TimeSpan.FromSeconds(0.5));
        string actionName = Constants.SoldierActionNames.Spin;

        foreach (Soldier soldier in aiSoldiers)
        {
            while (soldier.CanDoAction(actionName))
            {
                await soldier.DoAction(soldier.transform.position, Constants.SoldierActionNames.Spin);
                await Task.Delay(TimeSpan.FromSeconds(0.3));
            }
        }
        await Task.Delay(TimeSpan.FromSeconds(0.5));
        TurnController.Instance.TriggerNextTurn();
    }
}
