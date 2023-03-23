using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnVisualController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentTurnCountText;
    [SerializeField] private TextMeshProUGUI _whoseTurnText;
    private Button _endTurnButton;

    private void Awake()
    {
        this._endTurnButton = GetComponentInChildren<Button>();
    }

    public void SetShowEndTurnButton(bool showButton) => this._endTurnButton.gameObject.SetActive(showButton);

    public void SetCurrentTurn(int turnCount, bool isPlayerTurn)
    {
        this._currentTurnCountText.text = $"TURN {turnCount}";
        this._whoseTurnText.text = $"({(isPlayerTurn ? "Your" : "Enemy")} Turn)";
    }

    public void SetVisual(bool showVisual)
    {
        this.SetShowEndTurnButton(showVisual);
        this._currentTurnCountText.gameObject.SetActive(showVisual);
        this._whoseTurnText.gameObject.SetActive(showVisual);
    }
}
