using UnityEngine;

public class GameSceneVisualController : MonoBehaviour
{
    [SerializeField] private TurnVisualController _turnVisualController;
    [SerializeField] private SoldiersActionVisualController _soldiersActionVisualController;
    [SerializeField] private MultiplayerVisualController _multiplayerVisualController;
    [SerializeField] private GameObject _singlePlayerPlayerObject;

    private void Start()
    {
        bool isMultiplayer = MultiplayerManager.Instance.IsMultiplayer;
        this._turnVisualController.SetVisual(!isMultiplayer);
        this._soldiersActionVisualController.SetVisual(!isMultiplayer);

        if (!isMultiplayer)
        {
            this._singlePlayerPlayerObject.SetActive(true);
            this._multiplayerVisualController.SetVisual(false);
        }
        MultiplayerManager.Instance.OnGameSceneLoad();
    }
}
