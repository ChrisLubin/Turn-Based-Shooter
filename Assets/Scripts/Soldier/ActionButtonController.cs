using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonController : MonoBehaviour
{
    private TextMeshProUGUI _textMeshPro;
    private Button _button;
    private string _actionName;
    public event Action<ActionButtonController, string> OnClick;
    private Image _selectedVisual;

    private void Awake()
    {
        this._button = GetComponentInChildren<Button>();
        this._textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        this._selectedVisual = GetComponentsInChildren<Image>().First(img => img.transform != this._button.transform);
        this._selectedVisual.gameObject.SetActive(false);
    }

    private void Start()
    {
        this._button.onClick.AddListener(() => this.OnClick?.Invoke(this, this._actionName));
    }

    public void SetSelectedVisual(bool showVisual) => this._selectedVisual.gameObject.SetActive(showVisual);

    public void SetActionName(string actionName)
    {
        this._actionName = actionName;
        this._textMeshPro.text = actionName;
    }
}
