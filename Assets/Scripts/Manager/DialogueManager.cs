using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public event Action<NPCDialogueModel, NPCController> OnDialogueStarted;
    public event Action OnDialogueContinued;
    public event Action OnDialogueEnded;

    private NPCDialogueModel dialogueModel;
    private NPCController currentSpeaker;
    private int currentIndex;

    public bool IsDialogueActive { get; private set; }

    public string CurrentConversationId { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public int GetCurrentDialogueEntryCount()
    {
        return dialogueModel?.dialogues?.Count ?? 0;
    }

    public DialogueEntry GetCurrentEntry()
    {
        if (dialogueModel == null || currentIndex >= dialogueModel.dialogues.Count) return null;
        return dialogueModel.dialogues[currentIndex];
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }

    public void StartDialogue(NPCDialogueModel model, NPCController speaker)
    {
        if (model == null) return;

        dialogueModel = model;
        currentSpeaker = speaker;
        currentIndex = 0;
        IsDialogueActive = true;

        CurrentConversationId = model.conversationId;

        Debug.Log("start dialog");
        OnDialogueStarted?.Invoke(model, speaker);
        OnDialogueContinued?.Invoke();
    }

    public void ContinueDialogue()
    {
        if (!IsDialogueActive || dialogueModel == null) return;

        currentIndex++;

        if (currentIndex >= dialogueModel.dialogues.Count)
        {
            // ✅ If this was the final line of the "dayat_complete" dialogue, mark game as completed
            if (CurrentConversationId == "dayat_complete")
            {
                Debug.Log("[DialogueManager] All dialogues in 'dayat_complete' shown. Marking game as completed.");
                GameProgressManager.Instance.CompletedAllGameObjective = true;

                // Directly complete Mission 4 if active
                if (MissionManager.Instance.CurrentMission != null &&
                    MissionManager.Instance.CurrentMission.id == 4)
                {
                    Debug.Log("[DialogueManager] Completing Mission 4 directly from dialogue.");
                    GameStateManager.Instance.SetState(GameState.Gameplay);
                    MissionManager.Instance.CompleteCurrentMission();
                }
            }


            EndDialogue();
            return;
        }

        OnDialogueContinued?.Invoke();
    }

    private void EndDialogue()
    {
        IsDialogueActive = false;
        OnDialogueEnded?.Invoke();

        currentSpeaker?.OnDialogueEnded();
        dialogueModel = null;
        currentSpeaker = null;
        currentIndex = 0;
        CurrentConversationId = null;
    }
}
