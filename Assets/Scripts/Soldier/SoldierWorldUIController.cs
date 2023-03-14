using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoldierWorldUIController : MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    private TextMeshProUGUI _actionPointsText;
    private SoldierHealthController _healthController;
    private Soldier _soldier;

    private void Awake()
    {
        this._actionPointsText = GetComponentInChildren<TextMeshProUGUI>();
        this._soldier = GetComponentInParent<Soldier>();
        this._healthController = GetComponentInParent<SoldierHealthController>();
        this._soldier.OnActionPointsChange += this.UpdateActionPoints;
        this._healthController.OnHealthChange += this.UpdateHealthBar;
    }

    private void Start()
    {
        this.UpdateActionPoints(this._soldier.GetActionPoints());
        this.UpdateHealthBar(this._healthController.GetHealth());
    }

    public void UpdateHealthBar(int newHealth) => this._healthBar.fillAmount = (float)newHealth / 100;
    private void UpdateActionPoints(int currentActionPoints) => this._actionPointsText.text = currentActionPoints.ToString();
}
