using TMPro;
using UnityEngine;

public class GridTileVisualController : MonoBehaviour
{
    private MeshRenderer _selectedVisual;
    private TextMeshPro _textMeshPro;

    private void Awake()
    {
        this._textMeshPro = GetComponentInChildren<TextMeshPro>();
        this._selectedVisual = GetComponentInChildren<MeshRenderer>();
        this.SetVisual(false);
    }

    public void SetText(string text) => this._textMeshPro.text = text;
    public void SetVisual(bool showVisual) => this._selectedVisual.gameObject.SetActive(showVisual);
}
