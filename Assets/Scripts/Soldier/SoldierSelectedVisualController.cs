using UnityEngine;

public class SoldierSelectedVisualController : MonoBehaviour
{
    private Soldier _soldier;
    private CircleAnimation _selectedVisual;

    private void Awake()
    {
        this._soldier = GetComponentInParent<Soldier>();
        this._selectedVisual = GetComponentInChildren<CircleAnimation>();
    }

    private void Start()
    {
        Soldier selectedSoldier = SoldiersActionController.Instance.GetSelectedSoldier();
        SoldiersActionController.Instance.OnSelectedSoldierChange += this.OnSelectedSoldierChange;
        HandleShowVisual(selectedSoldier);
    }

    private void OnSelectedSoldierChange(Soldier selectedSoldier)
    {
        HandleShowVisual(selectedSoldier);
    }

    private void HandleShowVisual(Soldier selectedSoldier)
    {
        this._selectedVisual.gameObject.SetActive(this._soldier == selectedSoldier);
    }
}
