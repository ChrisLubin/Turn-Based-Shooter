using Unity.Netcode;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    public static MultiplayerManager Instance { get; private set; }
    [SerializeField] private GameObject _playerOrbPrefab;
    [field: SerializeField] public bool IsMultiplayer { get; private set; }
    public enum MultiplayerState
    {
        Lobby,
        HostWaitingForPlayer,
        InGame,
    }
    public MultiplayerState State { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 45;
            DontDestroyOnLoad(this);
            return;
        }

        Destroy(gameObject);
    }

    public void SetIsMultiplayer(bool isMultiplayer) => this.IsMultiplayer = isMultiplayer;

    private void OnStateChange(MultiplayerState newState)
    {
        if (newState == MultiplayerState.InGame)
        {
            // Loading into game
            Debug.Log($"Is host: {NetworkManager.Singleton.IsHost}");
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
        }
    }

    public void SetState(MultiplayerState state)
    {
        this.State = state;
        this.OnStateChange(state);
    }
}
