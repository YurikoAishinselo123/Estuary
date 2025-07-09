using UnityEngine;
using TMPro;
using System.Collections;

public class GarbageFactUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text factText;
    [SerializeField] private float fadeDuration = 1f;

    private Coroutine currentFadeRoutine;

    void Start()
    {
        canvasGroup.gameObject.SetActive(false);

        if (DetectionManager.Instance != null)
        {
            DetectionManager.Instance.OnFactDetected += HandleFactDetected;
        }
        else
        {
            Debug.LogWarning("DetectionManager.Instance is null in GarbageFactUI Start()");
        }
    }


    // private void OnEnable()
    // {
    //     DetectionManager.Instance.OnFactDetected += HandleFactDetected;
    // }

    private void OnDisable()
    {
        DetectionManager.Instance.OnFactDetected -= HandleFactDetected;
    }

    private void HandleFactDetected(string fact)
    {
        if (string.IsNullOrEmpty(fact))
        {
            Hide();
        }
        else
        {
            Show(fact);
        }
    }

    private void Show(string fact)
    {
        Debug.LogWarning("Show fact on UI");
        canvasGroup.gameObject.SetActive(true);

        factText.text = fact;

        if (currentFadeRoutine != null)
            StopCoroutine(currentFadeRoutine);

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void Hide()
    {
        if (currentFadeRoutine != null)
            StopCoroutine(currentFadeRoutine);

        currentFadeRoutine = StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {

        float time = 0f;
        float startAlpha = canvasGroup.alpha;

        while (time < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.gameObject.SetActive(false);

    }
}
