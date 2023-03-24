using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerVisualController : NetworkBehaviour
{
    [SerializeField] private Image _panel;
    [SerializeField] private Button _createRoomButton;
    [SerializeField] private TMP_InputField _joinRoomInput;
    [SerializeField] private Button _joinRoomButton;
    [SerializeField] private TMP_Text _statusText;
    public Action OnCreateRoomButtonClick;
    public Action OnJoinRoomButtonClick;

    private void Awake()
    {
        this.SetVisual(false);
    }

    private void Start()
    {
        this._createRoomButton.onClick.AddListener(() =>
        {
            this.OnCreateRoomButtonClick?.Invoke();
        });
        this._joinRoomButton.onClick.AddListener(() =>
        {
            this.OnJoinRoomButtonClick?.Invoke();
        });
    }

    private void SetPanelVisual(bool showVisual) => this._panel.gameObject.SetActive(showVisual);
    public void SetStatusTextVisual(bool showText) => this._statusText.gameObject.SetActive(showText);
    public void SetStatusGameCode(string code) => this._statusText.text = $"Your Code Is: {code}\n\nWaiting For A Player To Join...";
    public void SetStatusText(string text) => this._statusText.text = text;
    public string GetInputText() => this._joinRoomInput.text;

    public void SetVisual(bool showVisual)
    {
        this.SetPanelVisual(showVisual);
        this.SetInteractableVisual(showVisual);
        if (!showVisual)
        {
            this.SetStatusTextVisual(false);
        }
    }

    public void SetInteractableVisual(bool showVisual)
    {
        this._createRoomButton.gameObject.SetActive(showVisual);
        this._joinRoomInput.gameObject.SetActive(showVisual);
        this._joinRoomButton.gameObject.SetActive(showVisual);
    }
}
