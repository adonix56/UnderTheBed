using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static GameState CurrentGameState { get; private set; }
    private GameState StateBeforePause;

    public enum GameState
    {
        MainMenu, Paused, Level1
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void Pause() { 
        Time.timeScale = 0.0f;
        StateBeforePause = CurrentGameState;
        CurrentGameState = GameState.Paused;
    }

    public void UnPause()
    {
        Time.timeScale = 1.0f;
        CurrentGameState = StateBeforePause;
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1.0f;
        CurrentGameState = GameState.MainMenu;
    }
}
