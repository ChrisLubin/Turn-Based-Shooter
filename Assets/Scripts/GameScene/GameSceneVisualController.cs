using Unity.Netcode;
using UnityEngine;

public class GameSceneVisualController : MonoBehaviour
{
    [SerializeField] private TurnVisualController _turnVisualController;
    [SerializeField] private SoldiersActionVisualController _soldiersActionVisualController;
    [SerializeField] private MultiplayerVisualController _multiplayerVisualController;
    [SerializeField] private GameObject _singlePlayerPlayerObject;

    private void Start()
    {
        if (!MultiplayerManager.Instance.IsMultiplayer)
        {
            return;
        }

        this._turnVisualController.SetVisual(false);
        this._soldiersActionVisualController.SetVisual(false);
        this._multiplayerVisualController.SetVisual(true);
    }

    public void OnGameStart()
    {
        this._turnVisualController.SetVisual(true);

        if (!MultiplayerManager.Instance.IsMultiplayer)
        {
            this._singlePlayerPlayerObject.SetActive(true);
            this._soldiersActionVisualController.SetVisual(true);
            this._multiplayerVisualController.SetVisual(false);
        }
        else
        {
            // Host goes first
            bool isHost = NetworkManager.Singleton.IsHost;
            this._soldiersActionVisualController.SetVisual(isHost);
        }
    }
}
