using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class GameSceneController : NetworkBehaviour
{
    public static GameSceneController Instance;
    private GameSceneVisualController _visual;
    [SerializeField] private Transform _soldierContainer;
    [SerializeField] private Transform _friendlySoldierPrefab;
    [SerializeField] private Transform _enemySoldierPrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one GameSceneController! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        this._visual = GetComponent<GameSceneVisualController>();
        MultiplayerManager.Instance.OnStartGameRequest += this.StartGameServerRpc;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        MultiplayerManager.Instance.OnStartGameRequest -= this.StartGameServerRpc;
    }

    private void Start()
    {
        MultiplayerManager.Instance.OnGameSceneLoad();
    }

    [ServerRpc]
    public void StartGameServerRpc()
    {
        int soldierSpawnCount = 8;

        // Spawn soldiers
        for (var i = 0; i < soldierSpawnCount; i++)
        {
            bool isHostSoldier = i < soldierSpawnCount / 2;
            Vector3 startingPosition = isHostSoldier ? Vector3.zero : MultiplayerManager.Instance.IsMultiplayer ? new Vector3(26, 0, 28) : new Vector3(22, 0, 14);
            Vector3 spawnDirection = isHostSoldier ? Vector3.right : Vector3.left;
            float gridTileSize = 2f;
            ulong hostClientId = this.OwnerClientId;
            var nonHostClientIdAttempt = NetworkManager.Singleton.ConnectedClientsList.FirstOrDefault(client => client.ClientId != this.OwnerClientId)?.ClientId;
            ulong nonHostClientId = (ulong)(nonHostClientIdAttempt != null ? nonHostClientIdAttempt : 987654321);
            ulong ownerClientId = isHostSoldier ? hostClientId : nonHostClientId;

            this.SpawnSoldierClientRpc(startingPosition + (spawnDirection * i * gridTileSize), ownerClientId, isHostSoldier ? Quaternion.identity : Quaternion.Euler(0, 180f, 0));
        }
        this.StartGameClientRpc();
    }

    [ClientRpc]
    public void StartGameClientRpc()
    {
        this._visual.OnGameStart();
        TurnController.Instance.OnGameStart();
        SoldiersActionController.Instance.OnGameStart();
        LevelGrid.Instance.OnGameStart();
    }

    [ClientRpc]
    public void SpawnSoldierClientRpc(Vector3 worldPosition, ulong ownerId, Quaternion rotation) => Instantiate(ownerId == NetworkManager.Singleton.LocalClientId ? this._friendlySoldierPrefab : this._enemySoldierPrefab, worldPosition, rotation, this._soldierContainer);
}
