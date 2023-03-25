using Unity.Netcode;
using UnityEngine;

public class GameSceneController : NetworkBehaviour
{
    public static GameSceneController Instance;
    private GameSceneVisualController _visual;

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
    public void StartGameServerRpc() => this.StartGameClientRpc();

    [ClientRpc]
    public void StartGameClientRpc()
    {
        this._visual.OnGameStart();
        TurnController.Instance.OnGameStart();
    }
}
