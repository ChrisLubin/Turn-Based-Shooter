using System;
using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{
    public static TurnController Instance { get; private set; }
    private int _currentTurn;
    private TurnVisualController _visualController;
    private Button _endTurnButton;
    public Action<bool> OnTurnEnd;
    private bool _isPlayerTurn = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            this._visualController = GetComponentInChildren<TurnVisualController>();
            this._endTurnButton = GetComponentInChildren<Button>();
            this._currentTurn = 1;
            this.UpdateCurrentTurn();
            return;
        }

        Debug.LogError("There's more than one TurnController! " + transform + " - " + Instance);
        Destroy(gameObject);
    }

    void Start()
    {
        this._endTurnButton.onClick.AddListener(this.TriggerNextTurn);
    }

    public void SetShowEndTurnButton(bool showButton) => this._visualController.SetShowEndTurnButton(showButton);
    public bool GetIsPlayerTurn() => this._isPlayerTurn;

    private void UpdateCurrentTurn()
    {
        this._visualController.SetCurrentTurn(this._currentTurn, this._isPlayerTurn);
        this._visualController.SetShowEndTurnButton(this._isPlayerTurn);
    }

    public void TriggerNextTurn()
    {
        this._currentTurn++;
        this._isPlayerTurn = !this._isPlayerTurn;
        this.UpdateCurrentTurn();

        Soldier[] soldiers = GameObject.FindObjectsOfType<Soldier>();

        foreach (Soldier soldier in soldiers)
        {
            soldier.ResetActionPoints();
        }

        this.OnTurnEnd?.Invoke(this._isPlayerTurn);
    }
}
