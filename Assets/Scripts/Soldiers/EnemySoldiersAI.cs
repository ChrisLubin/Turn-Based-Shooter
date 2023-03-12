using System;
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

        await Task.Delay(TimeSpan.FromSeconds(2));
        TurnController.Instance.TriggerNextTurn();
    }
}
