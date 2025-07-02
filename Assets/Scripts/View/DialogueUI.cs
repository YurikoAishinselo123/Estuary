using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    public bool IsShowing => dialoguePanel.activeSelf;

    private void Awake()
    {
        // Always keep this enabled to ensure event subscriptions are active
        dialoguePanel.SetActive(false);

        // Subscribe early to avoid missing events
        DialogueManager.Instance.OnDialogueStarted += HandleStart;
        DialogueManager.Instance.OnDialogueContinued += DisplayNext;
        DialogueManager.Instance.OnDialogueEnded += Hide;
    }

    private void OnDestroy()
    {
        // Clean up
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStarted -= HandleStart;
            DialogueManager.Instance.OnDialogueContinued -= DisplayNext;
            DialogueManager.Instance.OnDialogueEnded -= Hide;
        }
    }

    private void HandleStart(NPCDialogueModel model, NPCController speaker)
    {
        dialoguePanel.SetActive(true);
        speaker?.OnDialogueStarted();
    }

    private void DisplayNext()
    {
        var entry = DialogueManager.Instance.GetCurrentEntry();
        if (entry == null)
        {
            Hide();
            return;
        }

        speakerNameText.text = entry.speaker;
        dialogueText.text = entry.text;
    }

    private void Hide()
    {
        dialoguePanel.SetActive(false);
    }
}
