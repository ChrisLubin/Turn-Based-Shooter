using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerOverlayVisualController : NetworkBehaviour
{
    [SerializeField] private Image _panel;
    [SerializeField] private Button _createRoomButton;
    [SerializeField] private TMP_InputField _joinRoomInput;
    [SerializeField] private Button _joinRoomButton;
    [SerializeField] private TMP_Text _statusText;

    private void Awake()
    {
        this.SetVisual(false);
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += this.OnClientConnected;
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

    public override void OnDestroy()
    {
        base.OnDestroy();
        NetworkManager.Singleton.OnClientConnectedCallback -= this.OnClientConnected;
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

    private void OnClientConnected(ulong clientId)
    {
        if (this.IsHost && this.OwnerClientId == clientId)
        {
            // Ignore when host first connects
            return;
        }

        this.SetVisual(false);
        MultiplayerManager.Instance.SetState(MultiplayerManager.MultiplayerState.InGame);
        if (this.IsHost)
        {
            Debug.Log($"Client ID {clientId} connected to you.");
        }
        else
        {
            Debug.Log($"You joined with a client ID of {clientId}.");
        }
    }
}
