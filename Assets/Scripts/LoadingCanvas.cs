using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingCanvas : MonoBehaviour
{
    private const string HIDE = "Hide";
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void StartLoad()
    {
        SceneManager.LoadScene(Loader.SceneName.LoadingScene.ToString());
    }

    public void SetupLoadScene(Loader.SceneName sceneName)
    {
        Loader.SetupLoadScene(sceneName);
        animator.SetTrigger(HIDE);
    }
}
