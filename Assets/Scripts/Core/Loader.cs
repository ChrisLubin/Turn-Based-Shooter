using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene,
    }
    private static Scene _targetScene;

    public static void Load(Scene targetScene)
    {
        Loader._targetScene = targetScene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(Loader._targetScene.ToString());
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 100;
    }
}
