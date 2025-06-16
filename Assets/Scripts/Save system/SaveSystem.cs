using UnityEngine;

public static class SaveSystem
{
    private const string ChapterKey = "CurrentChapter";

    public static int GetCurrentChapter()
    {
        return PlayerPrefs.GetInt(ChapterKey, 1); // Default to Chapter 1
    }

    public static void SetCurrentChapter(int chapter)
    {
        PlayerPrefs.SetInt(ChapterKey, chapter);
        PlayerPrefs.Save();
    }

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteKey(ChapterKey);
    }
}
