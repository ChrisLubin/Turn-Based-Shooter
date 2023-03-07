using System;
using UnityEngine;

public class SoldiersActionController : MonoBehaviour
{
    [SerializeField] private Soldier _selectedSoldier;
    public static SoldiersActionController Instance
    {
        get; private set;
    }
    public event Action OnSelectedSoldierChange;
    private bool _isBusy;
    private SoldiersActionVisualController _visualController;

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
        this._visualController.UpdateSoldierActionButtons(this._selectedSoldier);
    }

    private void Update()
    {
        // this.plane.gameObject.SetActive(this._isBusy);
    }

    private void OnRightClick()
    {
        this._isBusy = true;
        this._selectedSoldier.DoSpinAction(this.OnActionDone);
    }

    private void OnActionDone() => this._isBusy = false;

    public void MoveSelectedSoldierToPosition(Vector3 to)
    {
        this._isBusy = true;
        this._selectedSoldier.DoMoveAction(this.OnActionDone, to);
    }
    public Soldier GetSelectedSolder() => this._selectedSoldier;

    private void OnLayerLeftClick(int layerMaskId, GameObject gameObject)
    {
        if (layerMaskId != (int)Constants.LayerMaskIds.Soldier)
        {
            return;
        }

        bool gotSoldier = gameObject.TryGetComponent<Soldier>(out Soldier soldier);
        if (gotSoldier)
        {
            HandleSoldierSelection(soldier);
        }
    }

    private void HandleSoldierSelection(Soldier soldier)
    {
        if (this._selectedSoldier != soldier)
        {
            this._selectedSoldier.SetVisual(false);
            this._selectedSoldier = soldier;
            this._selectedSoldier.SetVisual(true);
            this._visualController.UpdateSoldierActionButtons(this._selectedSoldier);
            this.OnSelectedSoldierChange?.Invoke();
        }
    }
}
