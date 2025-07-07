using UnityEngine;
using TMPro;
using System.Collections;

public class GuidanceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI guidanceText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;

    private Coroutine fadeCoroutine;

    private void OnEnable()
    {
        if (GuidanceManager.Instance != null)
        {
            GuidanceManager.Instance.OnGuidanceChanged += ShowGuidanceWithFade;
        }

        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStarted += HandleDialogueStarted;
            DialogueManager.Instance.OnDialogueEnded += HandleDialogueEnded;
        }
    }

    private void OnDisable()
    {
        if (GuidanceManager.Instance != null)
        {
            GuidanceManager.Instance.OnGuidanceChanged -= ShowGuidanceWithFade;
        }

        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStarted -= HandleDialogueStarted;
            DialogueManager.Instance.OnDialogueEnded -= HandleDialogueEnded;
        }
    }

    private void HandleDialogueStarted(NPCDialogueModel model, NPCController speaker)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void HandleDialogueEnded()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }



    private void ShowGuidanceWithFade(string message)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeGuidance(message));
    }

    private IEnumerator FadeGuidance(string message)
    {
        // Fade out
        yield return FadeCanvasGroup(1f, 0f);

        // Change text
        guidanceText.text = message;
        Debug.Log($"[GuidanceUI] Showing guidance: {message}");

        // Fade in
        yield return FadeCanvasGroup(0f, 1f);
    }

    private IEnumerator FadeCanvasGroup(float from, float to)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = to;
    }
}
