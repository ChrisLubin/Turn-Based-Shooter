using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridTileVisualController : MonoBehaviour
{
    private MeshRenderer _selectedVisual;
    private TextMeshPro _textMeshPro;
    [Serializable]
    public struct GridTileMaterial
    {
        public Constants.GridTileColor color;
        public Material material;
    }
    [SerializeField] private List<GridTileMaterial> _gridTileMaterials;

    private void Awake()
    {
        this._textMeshPro = GetComponentInChildren<TextMeshPro>();
        this._selectedVisual = GetComponentInChildren<MeshRenderer>();
        this.SetVisual(false);
    }

    public void SetText(string text) => this._textMeshPro.text = text;
    private Material GetGridTileMaterial(Constants.GridTileColor color) => this._gridTileMaterials.Find(mat => mat.color == color).material;

    public void SetVisual(bool showVisual, Constants.GridTileColor color = Constants.GridTileColor.White)
    {
        this._selectedVisual.gameObject.SetActive(showVisual);
        this._selectedVisual.material = this.GetGridTileMaterial(color);
    }
}
