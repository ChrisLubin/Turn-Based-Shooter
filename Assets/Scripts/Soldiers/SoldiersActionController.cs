using System;
using System.Collections.Generic;
using System.Linq;
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
    public Constants.SoldierActionTargetTypes GetSelectedActionTargetType() => this._selectedSoldier.GetActionTargetType(this._selectedActionName);

    public void OnTurnEnd(bool isPlayerTurn)
    {
        bool isSelectedSoldierDead = this._selectedSoldier == null;

        if (isSelectedSoldierDead)
        {
            List<Soldier> friendlySoldiers = GameObject.FindObjectsOfType<Soldier>().Where(tempSoldier => !tempSoldier.GetIsEnemy()).ToList();
            if (friendlySoldiers.Count == 0)
            {
                this._selectedSoldier = null;
                return;
            }
            this.HandleSoldierSelection(friendlySoldiers[0]);
            this._visualController.SetActionPointsVisual(isPlayerTurn);
            this._visualController.UpdateActionPoints(this._selectedSoldier.GetActionPoints());
            return;
        }

        this._selectedSoldier.SetVisual(isPlayerTurn);
        this._visualController.SetActionPointsVisual(isPlayerTurn);
        this._visualController.SetButtonsVisual(isPlayerTurn && this._selectedSoldier.GetActionPoints() != 0);
        this._visualController.UpdateActionPoints(this._selectedSoldier.GetActionPoints());
    }

    private void OnActionDone()
    {
        Soldier[] friendlySoldiers = GameObject.FindObjectsOfType<Soldier>().Where(soldier => !soldier.GetIsEnemy()).ToArray();
        if (friendlySoldiers.Count() == 0)
        {
            this.IsBusy = false;
            this._visualController.SetButtonsVisual(false);
            this._visualController.SetActionPointsVisual(false);
            TurnController.Instance.SetShowEndTurnButton(true);
            this.OnActionCompleted?.Invoke();
            return;
        }

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
        if (!gotSoldier)
        {
            return;
        }

        HandleSoldierSelection(soldier);
    }

    private void HandleSoldierSelection(Soldier newSoldier)
    {
        if (this._selectedSoldier == newSoldier)
        {
            return;
        }
        if (this.IsBusy)
        {
            return;
        }
        if (newSoldier.GetIsEnemy())
        {
            return;
        }

        bool isSelectedSoldierDead = this._selectedSoldier == null;

        if (!isSelectedSoldierDead)
        {
            this._selectedSoldier.SetVisual(false);
        }

        this._selectedSoldier = newSoldier;
        newSoldier.SetVisual(true);
        this._visualController.UpdateSoldierActionButtons(newSoldier, out string firstActionName);
        this._visualController.UpdateActionPoints(newSoldier.GetActionPoints());
        this._visualController.SetButtonsVisual(newSoldier.GetActionPoints() != 0);
        this._selectedActionName = firstActionName;
        this.OnSelectedActionChange?.Invoke();
    }
}
