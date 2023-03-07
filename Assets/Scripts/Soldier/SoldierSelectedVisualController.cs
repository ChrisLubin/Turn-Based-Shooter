using UnityEngine;

public class SoldierSelectedVisualController : MonoBehaviour
{
    private CircleAnimation _selectedVisual;

    private void Awake()
    {
        this._selectedVisual = GetComponentInChildren<CircleAnimation>();
    }

    private void Start()
    {
        this.SetShowVisual(false);
    }

    public void SetShowVisual(bool showVisual) => this._selectedVisual.gameObject.SetActive(showVisual);
}
