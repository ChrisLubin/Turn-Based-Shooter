using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerOverlayVisualController : MonoBehaviour
{
    private Image _panel;
    [SerializeField] private Button _createRoomButton;
    [SerializeField] private TMP_InputField _joinRoomInput;
    [SerializeField] private Button _joinRoomButton;
    [SerializeField] private TMP_Text _statusText;

    private void Awake()
    {
        this._panel = GetComponentInChildren<Image>();
        this.SetVisual(false);
    }

    private void Start()
    {
        this._createRoomButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            this.SetInteractableVisual(false);
            this.SetStatusTextVisual(true);
            MultiplayerManager.Instance.SetState(MultiplayerManager.MultiplayerState.HostWaitingForPlayer);
        });
        this._joinRoomButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }

    private void SetPanelVisual(bool showVisual) => this._panel.gameObject.SetActive(showVisual);
    private void SetStatusTextVisual(bool showText) => this._statusText.gameObject.SetActive(showText);
    private void SetStatusText(string text) => this._statusText.text = text;

    public void SetVisual(bool showVisual)
    {
        this.SetPanelVisual(showVisual);
        this.SetInteractableVisual(showVisual);
        if (!showVisual)
        {
            this.SetStatusTextVisual(false);
        }
    }

    private void SetInteractableVisual(bool showVisual)
    {
        this._createRoomButton.gameObject.SetActive(showVisual);
        this._joinRoomInput.gameObject.SetActive(showVisual);
        this._joinRoomButton.gameObject.SetActive(showVisual);
    }
}
