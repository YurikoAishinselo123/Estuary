using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    public static PauseUI Instance { get; private set; }

    public GameObject PauseCanvas;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;

    public bool isPaused = false;

    void Awake()
    {
        // Singleton + DontDestroyOnLoad
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        PauseCanvas.SetActive(false);
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        if (InputManager.Instance.Back)
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        PauseCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        PauseCanvas.SetActive(false);
    }

    private void QuitGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        PauseCanvas.SetActive(false);
        LoadSceneManager.Instance.LoadScene(SceneName.Mainmenu);
        AudioManager.Instance.PlayMainThemeBacksound();
    }
}
