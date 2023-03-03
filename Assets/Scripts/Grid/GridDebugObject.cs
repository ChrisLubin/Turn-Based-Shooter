using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    private GridObject _gridObject;
    private TextMeshPro _textMeshPro;

    private void Awake()
    {
        this._textMeshPro = GetComponentInChildren<TextMeshPro>();
    }

    public void SetGridObject(GridObject gridObject)
    {
        this._gridObject = gridObject;
        this._textMeshPro.text = gridObject.ToString();
    }
}
