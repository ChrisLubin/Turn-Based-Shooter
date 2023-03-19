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
            Loader.Load(Loader.Scene.GameScene);
        });
        this._multiplayerButton.onClick.AddListener(() =>
        {
            Debug.Log($"Multiplayer");
        });
        this._quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
