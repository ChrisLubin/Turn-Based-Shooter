using UnityEngine;
using UnityEngine.UI;

public class MultiplayerOverlayVisualController : MonoBehaviour
{
    private Image _panel;

    private void Awake()
    {
        this._panel = GetComponentInChildren<Image>();
        this.SetVisual(false);
    }

    public void SetVisual(bool showVisual) => this._panel.gameObject.SetActive(showVisual);
}
