using UnityEngine;
using System;

public enum GameState
{
    Gameplay,
    Gameover,
    ChapterCompleted,
    Loading,
    Settings,
    Mainmenu,
    Cutscene,
    Dialogue,
    Paused
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public GameState CurrentState { get; private set; } = GameState.Gameplay;

    public bool IsGameplay => CurrentState == GameState.Gameplay;

    public event Action<GameState> OnStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetState(GameState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;
        OnStateChanged?.Invoke(CurrentState);
    }

    public void ResumeGameplay()
    {
        SetState(GameState.Gameplay);
    }
}
