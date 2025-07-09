using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    public bool IsShowing => dialoguePanel.activeSelf;

    private void OnEnable()
    {
        dialoguePanel.SetActive(false);

        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStarted += HandleStart;
            DialogueManager.Instance.OnDialogueContinued += DisplayNext;
            DialogueManager.Instance.OnDialogueEnded += Hide;
        }
        else
        {
            Debug.LogError("[DialogueUI] DialogueManager.Instance is null in OnEnable()");
        }
    }

    private void OnDestroy()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStarted -= HandleStart;
            DialogueManager.Instance.OnDialogueContinued -= DisplayNext;
            DialogueManager.Instance.OnDialogueEnded -= Hide;
        }
    }

    private void HandleStart(NPCDialogueModel model, NPCController speaker)
    {
        Debug.Log("show dialog canvas");
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

        var currentIndex = DialogueManager.Instance.GetCurrentIndex();

        // Show photo if it's Dayat and at index 1
        if (entry.speaker == "Asep" && currentIndex == 1)
        {
            var dirty = GameProgressManager.Instance.DirtyEnvironmentPhoto;
            var clean = GameProgressManager.Instance.CleanEnvironmentPhoto;

            if (dirty != null && clean != null)
            {
                PhotoDisplayUI.Instance.ShowPhotos(dirty, clean);
            }
        }
        else
        {
            PhotoDisplayUI.Instance.HidePhotos();
        }

        // // ✅ Check if it's the LAST line of the dialogue and it's the special one
        // var totalEntries = DialogueManager.Instance.GetCurrentDialogueEntryCount();
        // if (entry.speaker == "Asep" && currentIndex == totalEntries - 1)
        // {
        //     GameProgressManager.Instance.CompletedAllGameObjective = true;
        // }
    }


    private void Hide()
    {
        dialoguePanel.SetActive(false);
        PhotoDisplayUI.Instance.HidePhotos(); // hide juga kalau dialog selesai
    }
}
