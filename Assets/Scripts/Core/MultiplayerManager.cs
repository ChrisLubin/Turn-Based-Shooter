using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class MultiplayerManager : NetworkBehaviour
{
    public static MultiplayerManager Instance { get; private set; }
    [SerializeField] private GameObject _playerOrbPrefab;
    [field: SerializeField] public bool IsMultiplayer { get; private set; }
    [field: SerializeField] public bool IsLocalMultiplayer { get; private set; }
    public enum MultiplayerState
    {
        LobbyRelayNotConnected,
        LobbyRelayConnected,
        LobbyLocal,
        HostWaitingForPlayer,
        InGame,
    }
    public MultiplayerState State { get; private set; }
    private MultiplayerOverlayVisualController _visual;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 45;
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        if (!this.IsMultiplayer)
        {
            return;
        }

        NetworkManager.Singleton.OnClientConnectedCallback -= this.OnClientConnected;
    }

    public void SetIsMultiplayer(bool isMultiplayer) => this.IsMultiplayer = isMultiplayer;

    public void OnGameSceneLoad()
    {
        if (!this.IsMultiplayer)
        {
            return;
        }

        NetworkManager.Singleton.OnClientConnectedCallback += this.OnClientConnected;
        this._visual = GameObject.FindObjectOfType<MultiplayerOverlayVisualController>();
        this._visual.SetVisual(true);
        this._visual.SetInteractableVisual(false);
        this._visual.OnCreateRoomButtonClick += this.OnCreateRoomButtonClick;
        this._visual.OnJoinRoomButtonClick += this.OnJoinRoomButtonClick;
        this.SetState(this.IsLocalMultiplayer ? MultiplayerState.LobbyLocal : MultiplayerState.LobbyRelayNotConnected);
    }

    private void OnRelaySignIn()
    {
        Debug.Log($"Signed in as {AuthenticationService.Instance.PlayerId}");
        this._visual.SetInteractableVisual(true);
    }

    private void SetState(MultiplayerState state)
    {
        this.State = state;
        this.OnStateChange(state);
    }

    private async void OnStateChange(MultiplayerState newState)
    {
        switch (newState)
        {
            case MultiplayerState.LobbyLocal:
                this._visual.SetInteractableVisual(true);
                break;
            case MultiplayerState.LobbyRelayNotConnected:
                await UnityServices.InitializeAsync();
                AuthenticationService.Instance.SignedIn += this.OnRelaySignIn;
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                break;
            case MultiplayerState.HostWaitingForPlayer:
                if (this.IsLocalMultiplayer)
                {
                    this._visual.SetGameCode("(Not Needed)");
                }
                else
                {
                    AuthenticationService.Instance.SignedIn -= this.OnRelaySignIn;
                    this._visual.SetGameCode("Loading...");
                    try
                    {
                        int maxPlayersCount = 2;
                        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayersCount);
                        string gameCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                        this._visual.SetGameCode(gameCode);
                    }
                    catch (RelayServiceException e)
                    {
                        this._visual.SetGameCode("Something went wrong...");
                        Debug.LogError(e);
                    }
                }
                break;
            case MultiplayerState.InGame:
                // Loading into game
                if (!NetworkManager.Singleton.IsHost) { return; }

                // Host instantiates player objects
                foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
                {
                    Vector3 nonHostSpawnPoint = new(22, 0, 28);
                    GameObject playerOrb = Instantiate(this._playerOrbPrefab, clientId == NetworkManager.Singleton.LocalClientId ? Vector3.zero : nonHostSpawnPoint, Quaternion.identity);

                    if (clientId != NetworkManager.Singleton.LocalClientId)
                    {
                        // Rotate non-host player
                        playerOrb.transform.eulerAngles = new Vector3(0, -30f, 0);
                    }

                    playerOrb.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
                }
                break;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (this.IsHost && this.OwnerClientId == clientId)
        {
            // Ignore when host first connects
            return;
        }

        this._visual.SetVisual(false);
        this.SetState(MultiplayerManager.MultiplayerState.InGame);
        if (this.IsHost)
        {
            Debug.Log($"Client ID {clientId} connected to you.");
        }
        else
        {
            Debug.Log($"You joined with a client ID of {clientId}.");
        }
    }

    private void OnCreateRoomButtonClick()
    {
        NetworkManager.Singleton.StartHost();
        this._visual.SetInteractableVisual(false);
        this._visual.SetStatusTextVisual(true);
        MultiplayerManager.Instance.SetState(MultiplayerManager.MultiplayerState.HostWaitingForPlayer);
    }

    private void OnJoinRoomButtonClick()
    {
        if (MultiplayerManager.Instance.IsLocalMultiplayer)
        {
            NetworkManager.Singleton.StartClient();
        }
        else
        {

        }
    }
}
