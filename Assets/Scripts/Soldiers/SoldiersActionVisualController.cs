using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldiersActionVisualController : MonoBehaviour
{
    [SerializeField] private Transform _actionButtonContainerTransform;
    [SerializeField] private Transform _actionButtonPrefab;
    public event Action<string> OnButtonClick;
    private List<ActionButtonController> _actionButtons = new();

    public void UpdateSoldierActionButtons(Soldier soldier, out BaseAction firstAction)
    {
        this._actionButtons = new();

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
            actionButtonController.OnClick += this.OnActionChange;
            this._actionButtons.Add(actionButtonController);

            if (action == firstAction)
            {
                actionButtonController.SetSelectedVisual(true);
            }
        }
    }

    public void OnActionChange(ActionButtonController actionButton, string actionName)
    {
        foreach (ActionButtonController button in this._actionButtons)
        {
            button.SetSelectedVisual(false);
        }

        actionButton.SetSelectedVisual(true);
        this.OnButtonClick?.Invoke(actionName);
    }
}
