using UnityEngine;

public class CrosshairUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup container;

    private void Awake()
    {
        // Try to get CanvasGroup if not already added
        container = GetComponent<CanvasGroup>();
        if (container == null)
        {
            container = gameObject.AddComponent<CanvasGroup>();
        }
    }

    private void OnEnable()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStarted += HandleDialogueStarted;
            DialogueManager.Instance.OnDialogueEnded += HandleDialogueEnded;
        }
    }

    private void OnDisable()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStarted -= HandleDialogueStarted;
            DialogueManager.Instance.OnDialogueEnded -= HandleDialogueEnded;
        }
    }

    private void HandleDialogueStarted(NPCDialogueModel model, NPCController speaker)
    {
        SetVisible(false);
    }

    private void HandleDialogueEnded()
    {
        SetVisible(true);
    }

    private void SetVisible(bool visible)
    {
        container.alpha = visible ? 1f : 0f;
        container.interactable = visible;
        container.blocksRaycasts = visible;
    }
}
