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

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartDialogue(NPCDialogueModel model, NPCController speaker)
    {
        if (model == null) return;

        dialogueModel = model;
        currentSpeaker = speaker;
        currentIndex = 0;
        IsDialogueActive = true;

        OnDialogueStarted?.Invoke(model, speaker);
        OnDialogueContinued?.Invoke(); // 👈 Immediately show first line
    }

    public void ContinueDialogue()
    {
        if (!IsDialogueActive || dialogueModel == null) return;

        currentIndex++;

        if (currentIndex >= dialogueModel.dialogues.Count)
        {
            EndDialogue();
            return;
        }

        OnDialogueContinued?.Invoke();
    }

    public DialogueEntry GetCurrentEntry()
    {
        if (dialogueModel == null || currentIndex >= dialogueModel.dialogues.Count) return null;
        return dialogueModel.dialogues[currentIndex];
    }

    private void EndDialogue()
    {
        IsDialogueActive = false;
        OnDialogueEnded?.Invoke();

        currentSpeaker?.OnDialogueEnded();
        dialogueModel = null;
        currentSpeaker = null;
        currentIndex = 0;
    }
}
