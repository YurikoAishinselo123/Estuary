using UnityEngine;
using System;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }

    public event Action<Chapter> OnChapterLoaded;

    public Chapter CurrentChapter { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        int currentChapterIndex = SaveSystem.GetCurrentChapter();
        LoadChapter($"Chapter{currentChapterIndex}");
    }

    private void LoadChapter(string fileName)
    {
        CurrentChapter = MissionLoader.LoadChapter(fileName);

        if (CurrentChapter != null)
        {
            Debug.Log($"Loaded: {CurrentChapter.chapterTitle}");
            OnChapterLoaded?.Invoke(CurrentChapter);
        }
        else
        {
            Debug.LogError($"Failed to load chapter: {fileName}");
        }
    }

    public void CompleteCurrentChapterAndAdvance()
    {
        int currentChapter = SaveSystem.GetCurrentChapter();
        SaveSystem.SetCurrentChapter(currentChapter + 1);
        LoadChapter($"Chapter{currentChapter + 1}");
    }
}
