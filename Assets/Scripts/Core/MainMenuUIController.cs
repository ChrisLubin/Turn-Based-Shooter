using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] private Button _singleplayerButton;
    [SerializeField] private Button _multiplayerButton;
    [SerializeField] private Button _quitButton;

    private void Awake()
    {
        this._singleplayerButton.onClick.AddListener(() =>
        {
            MultiplayerManager.Instance.SetIsMultiplayer(false);
            Loader.Load(Loader.Scene.GameScene);
        });
        this._multiplayerButton.onClick.AddListener(() =>
        {
            MultiplayerManager.Instance.SetIsMultiplayer(true);
            Loader.Load(Loader.Scene.GameScene);
        });
        this._quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
