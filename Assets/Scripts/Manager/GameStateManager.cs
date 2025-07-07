using UnityEngine;
using System;

public class GameStateManager : MonoBehaviour
{
    
    public static GameStateManager Instance { get; private set; }

    public GameState CurrentState { get; private set; } = GameState.Gameplay;
    public MissionStage CurrentMissionStage { get; private set; } = MissionStage.FirstMeet;

    public bool IsGameplay => CurrentState == GameState.Gameplay;

    public event Action<GameState> OnStateChanged;
    public event Action<MissionStage> OnMissionStageChanged;

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

    private void Start()
    {
        SaveSystem.Instance?.LoadAll();

        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStarted += HandleDialogueStarted;
            DialogueManager.Instance.OnDialogueEnded += HandleDialogueEnded;
        }
        else
        {
            Debug.LogWarning("DialogueManager.Instance not found in GameStateManager Start()");
        }
    }

    private void OnDestroy()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStarted -= HandleDialogueStarted;
            DialogueManager.Instance.OnDialogueEnded -= HandleDialogueEnded;
        }
    }

    private void HandleDialogueStarted(NPCDialogueModel model, NPCController speaker)
    {
        SetState(GameState.Dialogue);
        Debug.Log("Current State");
    }

    private void HandleDialogueEnded()
    {
        ResumeGameplay();
    }

    public void SetState(GameState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;
        OnStateChanged?.Invoke(CurrentState);
        Debug.Log($"[GameStateManager] GameState changed to: {CurrentState}");
    }

    public void ResumeGameplay()
    {
        SetState(GameState.Gameplay);
    }

    public void SetMissionStage(MissionStage newStage)
    {
        if (newStage > CurrentMissionStage)
        {
            Debug.Log($"[GameStateManager] MissionStage changed: {CurrentMissionStage} -> {newStage}");
            CurrentMissionStage = newStage;
            OnMissionStageChanged?.Invoke(CurrentMissionStage);
        }
    }
}
