using UnityEngine;

public class SoldiersActionVisualController : MonoBehaviour
{
    [SerializeField] private Transform _actionButtonContainerTransform;
    [SerializeField] private Transform _actionButtonPrefab;

    public void UpdateSoldierActionButtons(Soldier soldier)
    {
        foreach (Transform actionButtonTransform in _actionButtonContainerTransform)
        {
            Destroy(actionButtonTransform.gameObject);
        }

        foreach (BaseAction baseAction in soldier.GetBaseActions())
        {
            Transform actionButtonTransform = Instantiate(_actionButtonPrefab, _actionButtonContainerTransform);
            ActionButtonController actionButtonController = actionButtonTransform.GetComponent<ActionButtonController>();
            actionButtonController.SetBaseAction(baseAction);
        }
    }
}
