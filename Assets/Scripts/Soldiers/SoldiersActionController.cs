using System;
using UnityEngine;

public class SoldiersActionController : MonoBehaviour
{
    [SerializeField] private Soldier _selectedSoldier;
    public static SoldiersActionController Instance { get; private set; }
    public event Action OnActionCompleted;
    public event Action OnSelectedActionChange;
    public event Action OnSelectedSoldierHasNoActionPoints;
    public bool IsBusy { get; private set; }
    private SoldiersActionVisualController _visualController;
    private string _selectedActionName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            this._visualController = GetComponentInChildren<SoldiersActionVisualController>();
            return;
        }

        Debug.LogError("There's more than one SoldierActionController! " + transform + " - " + Instance);
        Destroy(gameObject);
    }

    private void Start()
    {
        GlobalMouse.Instance.OnLayerLeftClick += this.OnLayerLeftClick;
        TurnController.Instance.OnTurnEnd += this.OnTurnEnd;
        this._selectedSoldier.SetVisual(true);
        this._visualController.OnButtonClick += this.OnActionChange;
        this._visualController.UpdateSoldierActionButtons(this._selectedSoldier, out string firstActionName);
        this._selectedActionName = firstActionName;
        this._visualController.UpdateActionPoints(this._selectedSoldier.GetActionPoints());
    }

    public Soldier GetSelectedSolder() => this._selectedSoldier;
    public string GetSelectedActionName() => this._selectedActionName;
    public int GetSelectedActionEffectiveDistance() => this._selectedSoldier.GetActionEffectiveDistance(this._selectedActionName);

    public void OnTurnEnd(bool isPlayerTurn)
    {
        this._selectedSoldier.SetVisual(isPlayerTurn);
        this._visualController.SetActionPointsVisual(isPlayerTurn);
        this._visualController.SetButtonsVisual(isPlayerTurn && this._selectedSoldier.GetActionPoints() != 0);
        this._visualController.UpdateActionPoints(this._selectedSoldier.GetActionPoints());
    }

    private void OnActionDone()
    {
        this.IsBusy = false;
        this._visualController.SetButtonsVisual(this._selectedSoldier.GetActionPoints() != 0);
        this.OnActionCompleted?.Invoke();
        TurnController.Instance.SetShowEndTurnButton(true);
    }

    public void DoAction(Vector3 to)
    {
        if (this.IsBusy)
        {
            return;
        }

        int actionCost = this._selectedSoldier.GetActionCost(this._selectedActionName);
        int soldierActionPoints = this._selectedSoldier.GetActionPoints();

        if (actionCost > soldierActionPoints)
        {
            return;
        }

        this.IsBusy = true;
        this._selectedSoldier.DoAction(this.OnActionDone, to, this._selectedActionName);
        this._visualController.UpdateActionPoints(this._selectedSoldier.GetActionPoints());
        this._visualController.SetButtonsVisual(false);
        TurnController.Instance.SetShowEndTurnButton(false);
        if (this._selectedSoldier.GetActionPoints() == 0)
        {
            this.OnSelectedSoldierHasNoActionPoints?.Invoke();
        }
    }

    private void OnActionChange(string actionName)
    {
        this._selectedActionName = actionName;
        this.OnSelectedActionChange?.Invoke();
    }

    private void OnLayerLeftClick(int layerMaskId, GameObject gameObject)
    {
        if (layerMaskId != (int)Constants.LayerMaskIds.Soldier)
        {
            return;
        }
        if (!TurnController.Instance.GetIsPlayerTurn())
        {
            return;
        }

        bool gotSoldier = gameObject.TryGetComponent<Soldier>(out Soldier soldier);

        if (gotSoldier && this._selectedSoldier == soldier && this._selectedSoldier.GetActionEffectiveDistance(this._selectedActionName) == 0)
        {
            // Action can only be done on soldier's current position
            this.DoAction(soldier.transform.position);
            return;
        }

        if (gotSoldier)
        {
            HandleSoldierSelection(soldier);
        }
    }

    private void HandleSoldierSelection(Soldier soldier)
    {
        if (this._selectedSoldier == soldier)
        {
            return;
        }
        if (this.IsBusy)
        {
            return;
        }
        if (soldier.GetIsEnemy())
        {
            return;
        }

        this._selectedSoldier.SetVisual(false);
        this._selectedSoldier = soldier;
        this._selectedSoldier.SetVisual(true);
        this._visualController.UpdateSoldierActionButtons(this._selectedSoldier, out string firstActionName);
        this._visualController.UpdateActionPoints(this._selectedSoldier.GetActionPoints());
        this._visualController.SetButtonsVisual(this._selectedSoldier.GetActionPoints() != 0);
        this._selectedActionName = firstActionName;
        this.OnSelectedActionChange?.Invoke();
    }
}
