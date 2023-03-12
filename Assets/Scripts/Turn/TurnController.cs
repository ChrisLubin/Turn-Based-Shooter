using System;
using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{
    public static TurnController Instance { get; private set; }
    private int _currentTurn;
    private TurnVisualController _visualController;
    private Button _endTurnButton;
    public Action OnTurnEnd;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            this._visualController = GetComponentInChildren<TurnVisualController>();
            this._endTurnButton = GetComponentInChildren<Button>();
            return;
        }

        Debug.LogError("There's more than one TurnController! " + transform + " - " + Instance);
        Destroy(gameObject);
    }

    void Start()
    {
        this._currentTurn = 1;
        this.UpdateCurrentTurn();
        this._endTurnButton.onClick.AddListener(this.OnEndTurnButtonClicked);
    }

    public void SetShowEndTurnButton(bool showButton) => this._visualController.SetShowEndTurnButton(showButton);
    private void UpdateCurrentTurn() => this._visualController.SetCurrentTurn(this._currentTurn);

    private void OnEndTurnButtonClicked()
    {
        this._currentTurn++;
        this.UpdateCurrentTurn();

        Soldier[] soldiers = GameObject.FindObjectsOfType<Soldier>();

        foreach (Soldier soldier in soldiers)
        {
            soldier.ResetActionPoints();
        }

        this.OnTurnEnd?.Invoke();
    }
}
