using System;
using UnityEngine;

public class SoldiersActionVisualController : MonoBehaviour
{
    [SerializeField] private Transform _actionButtonContainerTransform;
    [SerializeField] private Transform _actionButtonPrefab;
    public event Action<string> OnButtonClick;

    public void UpdateSoldierActionButtons(Soldier soldier, out BaseAction firstAction)
    {
        foreach (Transform actionButtonTransform in _actionButtonContainerTransform)
        {
            Destroy(actionButtonTransform.gameObject);
        }

        BaseAction[] actions = soldier.GetActions();
        firstAction = actions[0];

        foreach (BaseAction action in actions)
        {
            Transform actionButtonTransform = Instantiate(_actionButtonPrefab, _actionButtonContainerTransform);
            ActionButtonController actionButtonController = actionButtonTransform.GetComponent<ActionButtonController>();
            actionButtonController.SetAction(action);
            actionButtonController.OnClick += this.OnButtonClick;
        }
    }
}
