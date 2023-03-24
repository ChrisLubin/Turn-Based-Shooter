using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    public static MultiplayerManager Instance { get; private set; }
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
            DontDestroyOnLoad(this);
            return;
        }

        Destroy(gameObject);
    }

    public void SetIsMultiplayer(bool isMultiplayer) => this.IsMultiplayer = isMultiplayer;
    public void SetState(MultiplayerState state) => this.State = state;
}
