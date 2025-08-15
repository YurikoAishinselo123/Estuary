using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReflectionUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private bool isShowing = false;

    private void Start()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        MissionManager.Instance.OnReflectionMissionComplete += ShowReflection;
    }

    private void OnDestroy()
    {
        if (MissionManager.Instance != null)
            MissionManager.Instance.OnReflectionMissionComplete -= ShowReflection;
    }

    private IEnumerator FadeIn()
    {
        float time = 0;
        canvasGroup.gameObject.SetActive(true);

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void Update()
    {
        if (!isShowing) return;

        if (Input.anyKeyDown)
        {
            isShowing = false;
            StartCoroutine(FadeOutAndContinue());
        }
    }

    private void ShowReflection()
    {
        if (!GameProgressManager.Instance.HasShownReflection)
        {
            GameProgressManager.Instance.HasShownReflection = true;
            isShowing = true;
            GameStateManager.Instance.SetState(GameState.Reflection);
            StartCoroutine(ShowWithDelay());
        }
        
    }

    private IEnumerator ShowWithDelay()
    {
        yield return new WaitForSeconds(1f); // ⏳ Wait for 1 second
        yield return StartCoroutine(FadeIn());
    }


    private IEnumerator FadeOutAndContinue()
    {
        float time = 0;

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0;
        canvasGroup.gameObject.SetActive(false);

        // TODO: Add what happens after reflection (e.g., load next scene)
        GameStateManager.Instance.SetState(GameState.Gameplay);
        LoadSceneManager.Instance.LoadScene(SceneName.RuangKerja);
        Debug.Log("[ReflectionUI] Continue pressed. Proceeding...");
    }
}
