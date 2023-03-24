using UnityEngine;

public class GameSceneVisualController : MonoBehaviour
{
    [SerializeField] private TurnVisualController _turnVisualController;
    [SerializeField] private SoldiersActionVisualController _soldiersActionVisualController;
    [SerializeField] private MultiplayerOverlayVisualController _multiplayerOverlayVisualController;
    [SerializeField] private GameObject _singlePlayerPlayerObject;

    private void Start()
    {
        bool isMultiplayer = MultiplayerManager.Instance.IsMultiplayer;
        this._turnVisualController.SetVisual(!isMultiplayer);
        this._soldiersActionVisualController.SetVisual(!isMultiplayer);
        this._multiplayerOverlayVisualController.SetVisual(isMultiplayer);

        if (!isMultiplayer)
        {
            this._singlePlayerPlayerObject.SetActive(true);
        }
    }
}
