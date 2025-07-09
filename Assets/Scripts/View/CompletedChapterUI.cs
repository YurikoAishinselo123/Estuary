using UnityEngine;
using System.Collections;

public class CompletedChapterUI : MonoBehaviour
{
    public static CompletedChapterUI Instance { get; private set; }

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1.5f;
    private bool isShowing = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Hide initially
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
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


    public void Show()
    {
        isShowing = true;
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float time = 0f;

        // Enable interaction during fade
        canvasGroup.gameObject.SetActive(true);

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        while (time < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
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

        PhotoDisplayUI.Instance.HidePhoto();
        PhotoDisplayUI.Instance.HidePhotos();


        GameStateManager.Instance.SetState(GameState.Mainmenu);
        LoadSceneManager.Instance.LoadScene(SceneName.Mainmenu);
    }
}
