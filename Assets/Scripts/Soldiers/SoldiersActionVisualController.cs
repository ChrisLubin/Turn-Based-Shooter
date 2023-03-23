using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SoldiersActionVisualController : MonoBehaviour
{
    [SerializeField] private Transform _actionButtonContainerTransform;
    [SerializeField] private Transform _actionButtonPrefab;
    [SerializeField] private TextMeshProUGUI _actionPointsText;
    public event Action<string> OnButtonClick;
    private List<ActionButtonController> _actionButtons = new();

    public void UpdateActionPoints(int newActionPoints) => this._actionPointsText.text = $"Action Points: {newActionPoints}";
    public void SetButtonsVisual(bool showButtons) => this._actionButtons.ToList().ForEach(button => button.gameObject.SetActive(showButtons));
    public void SetActionPointsVisual(bool showActionPoints) => this._actionPointsText.gameObject.SetActive(showActionPoints);

    public void UpdateSoldierActionButtons(Soldier soldier, out string firstActionName)
    {
        this._actionButtons = new();

        foreach (Transform actionButtonTransform in _actionButtonContainerTransform)
        {
            Destroy(actionButtonTransform.gameObject);
        }

        string[] actionNames = soldier.GetActionNames();
        firstActionName = actionNames[0];

        foreach (string actionName in actionNames)
        {
            Transform actionButtonTransform = Instantiate(_actionButtonPrefab, _actionButtonContainerTransform);
            ActionButtonController actionButtonController = actionButtonTransform.GetComponent<ActionButtonController>();
            int actionPointsCost = soldier.GetActionCost(actionName);
            actionButtonController.SetAction(actionName, actionPointsCost);
            actionButtonController.OnClick += this.OnActionChange;
            this._actionButtons.Add(actionButtonController);

            if (actionName == firstActionName)
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

    public void SetVisual(bool showVisual)
    {
        this.SetButtonsVisual(showVisual);
        this.SetActionPointsVisual(showVisual);
    }
}
