using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public enum SceneName
    {
        LoadingScene, MainMenu, Level1
    }

    private static SceneName sceneToLoad = SceneName.LoadingScene;

    public static void SetupLoadScene(SceneName sceneName)
    {
        Loader.sceneToLoad = sceneName;
    }

    public void Start()
    {
        if (Loader.sceneToLoad == SceneName.LoadingScene)
        {
            SceneManager.LoadScene(SceneName.MainMenu.ToString());
        } else
        {
            SceneManager.LoadScene(Loader.sceneToLoad.ToString());
        }
    }
}
