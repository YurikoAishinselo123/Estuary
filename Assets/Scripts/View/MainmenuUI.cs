using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;


public class MainmenuUI : MonoBehaviour
{

    [SerializeField] private Button startButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button cutsceneButton;
    [SerializeField] private Button quitButton;


    void Awake()
    {
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
        newGameButton.onClick.AddListener(NewGame);
        cutsceneButton.onClick.AddListener(CutScene);
    }

    void Update()
    {
        // bool hasPlayedCutscene = PlayerPrefs.GetInt("HasNewGame", 0) == 1;
        // if (!hasPlayedCutscene)
        // {
        //     startButton.gameObject.SetActive(false);
        // }

    }

    void Start()
    {
        startButton.gameObject.SetActive(true);
        AudioManager.Instance.PlayMainThemeBacksound();
    }



    private void StartGame()
    {
        LoadSceneManager.Instance.LoadScene(SceneName.RuangKerja);
        AudioManager.Instance.PlayOfficeBacksound();
    }

    private void NewGame()
    {

    }

    private void CutScene()
    {
        LoadSceneManager.Instance.LoadScene(SceneName.Cutscene);
    }

    private void QuitGame()
    {
        LoadSceneManager.Instance.QuitGame();
    }
}