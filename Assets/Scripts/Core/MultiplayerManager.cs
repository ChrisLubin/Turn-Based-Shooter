using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    public static MultiplayerManager Instance { get; private set; }
    public bool IsMultiplayer { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            this.IsMultiplayer = true;
            Instance = this;
            DontDestroyOnLoad(this);
            return;
        }

        Destroy(gameObject);
    }

    public void SetIsMultiplayer(bool IsMultiplayer) => this.IsMultiplayer = IsMultiplayer;
}
