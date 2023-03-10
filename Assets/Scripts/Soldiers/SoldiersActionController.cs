using System;
using UnityEngine;

public class SoldiersActionController : MonoBehaviour
{
    [SerializeField] private Soldier _selectedSoldier;
    public static SoldiersActionController Instance
    {
        get; private set;
    }
    public event Action OnSelectedActionChange;
    private bool _isBusy;
    private SoldiersActionVisualController _visualController;
    private BaseAction _selectedAction;

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
        this._selectedSoldier.SetVisual(true);
        this._visualController.OnButtonClick += this.OnActionChange;
        this._visualController.UpdateSoldierActionButtons(this._selectedSoldier, out BaseAction firstAction);
        this._selectedAction = firstAction;
    }

    public Soldier GetSelectedSolder() => this._selectedSoldier;
    public BaseAction GetSelectedAction() => this._selectedAction;
    private void OnActionDone() => this._isBusy = false;

    public void DoAction(Vector3 to)
    {
        if (this._isBusy)
        {
            return;
        }

        this._isBusy = true;
        this._selectedAction.DoAction(this.OnActionDone, to);
    }

    private void OnActionChange(string actionName)
    {
        this._selectedAction = this._selectedSoldier.GetAction(actionName);
        this.OnSelectedActionChange?.Invoke();
    }

    private void OnLayerLeftClick(int layerMaskId, GameObject gameObject)
    {
        if (layerMaskId != (int)Constants.LayerMaskIds.Soldier)
        {
            return;
        }

        bool gotSoldier = gameObject.TryGetComponent<Soldier>(out Soldier soldier);

        if (gotSoldier && this._selectedSoldier == soldier && this._selectedAction.MaxEffectiveDistance == 0)
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

        this._selectedSoldier.SetVisual(false);
        this._selectedSoldier = soldier;
        this._selectedSoldier.SetVisual(true);
        this._visualController.UpdateSoldierActionButtons(this._selectedSoldier, out BaseAction firstAction);
        this._selectedAction = firstAction;
        this.OnSelectedActionChange?.Invoke();
    }
}
