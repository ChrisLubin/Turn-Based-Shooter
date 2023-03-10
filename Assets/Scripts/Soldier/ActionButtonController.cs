using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonController : MonoBehaviour
{
    private TextMeshProUGUI _textMeshPro;
    private Button _button;
    private BaseAction _action;
    public event Action<string> OnClick;

    private void Awake()
    {
        this._button = GetComponentInChildren<Button>();
        this._textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        this._button.onClick.AddListener(() => this.OnClick?.Invoke(this._action.ToString()));
    }

    public void SetAction(BaseAction action)
    {
        this._action = action;
        this._textMeshPro.text = action.ToString();
    }
}
