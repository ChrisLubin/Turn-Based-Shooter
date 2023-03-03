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

    private void Update()
    {
        this._textMeshPro.text = this._gridObject.ToString();
    }

    public void SetGridObject(GridObject gridObject)
    {
        this._gridObject = gridObject;
    }
}
