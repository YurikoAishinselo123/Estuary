using UnityEngine;

public class ChapterSaveHandler : ISaveableDataHandler
{
    private const string ChapterKey = "CurrentChapter";

    public int CurrentChapter { get; private set; } = 1;

    public void SaveData()
    {
        PlayerPrefs.SetInt(ChapterKey, CurrentChapter);
    }

    public void LoadData()
    {
        CurrentChapter = PlayerPrefs.GetInt(ChapterKey, 1);
    }

    public void SetChapter(int chapter)
    {
        CurrentChapter = chapter;
        SaveData();
    }
}
