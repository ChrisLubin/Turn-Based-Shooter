using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnVisualController : MonoBehaviour
{
    private TextMeshProUGUI _currentTurnText;
    private Button _endTurnButton;

    private void Awake()
    {
        this._currentTurnText = GetComponentInChildren<TextMeshProUGUI>();
        this._endTurnButton = GetComponentInChildren<Button>();
    }

    public void SetCurrentTurn(int turnCount) => this._currentTurnText.text = $"TURN {turnCount}";
    public void SetShowEndTurnButton(bool showButton) => this._endTurnButton.gameObject.SetActive(showButton);
}
