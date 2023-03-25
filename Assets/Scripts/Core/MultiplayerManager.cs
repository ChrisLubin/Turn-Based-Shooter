using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
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
        PlayerJoiningGame,
        InGame,
    }
    public MultiplayerState State { get; private set; }
    private MultiplayerVisualController _visual;
    public Action OnStartGameRequest;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There's more than one MultiplayerManager! " + transform + " - " + Instance);
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
            NetworkManager.Singleton.StartHost();
            this.OnStartGameRequest?.Invoke();
            return;
        }

        NetworkManager.Singleton.OnClientConnectedCallback += this.OnClientConnected;
        this._visual = GameObject.FindObjectOfType<MultiplayerVisualController>();
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
        string gameCode;
        RelayServerData relayServerData;

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
                    this._visual.SetStatusGameCode("(Not Needed)");
                    NetworkManager.Singleton.StartHost();
                    return;
                }

                AuthenticationService.Instance.SignedIn -= this.OnRelaySignIn;
                this._visual.SetStatusGameCode("Loading...");
                try
                {
                    int maxPlayersCount = 2;
                    Allocation createAllocation = await RelayService.Instance.CreateAllocationAsync(maxPlayersCount);
                    gameCode = await RelayService.Instance.GetJoinCodeAsync(createAllocation.AllocationId);
                    this._visual.SetStatusGameCode(gameCode);

                    relayServerData = new(createAllocation, "dtls");
                    NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
                    NetworkManager.Singleton.StartHost();
                }
                catch (RelayServiceException e)
                {
                    this._visual.SetStatusGameCode("Something went wrong...");
                    Debug.LogError(e);
                }
                break;
            case MultiplayerState.PlayerJoiningGame:
                if (this.IsLocalMultiplayer)
                {
                    NetworkManager.Singleton.StartClient();
                    return;
                }

                gameCode = this._visual.GetInputText().ToUpper();
                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(gameCode);
                relayServerData = new(joinAllocation, "dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
                NetworkManager.Singleton.StartClient();
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

                this.OnStartGameRequest?.Invoke();
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
        this._visual.SetInteractableVisual(false);
        this._visual.SetStatusTextVisual(true);
        MultiplayerManager.Instance.SetState(MultiplayerManager.MultiplayerState.HostWaitingForPlayer);
    }

    private void OnJoinRoomButtonClick()
    {
        this._visual.SetInteractableVisual(false);
        this._visual.SetStatusTextVisual(true);
        this._visual.SetStatusText("Joining Game...");
        MultiplayerManager.Instance.SetState(MultiplayerManager.MultiplayerState.PlayerJoiningGame);
    }
}
