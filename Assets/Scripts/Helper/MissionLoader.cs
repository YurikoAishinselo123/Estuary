using System.IO;
using UnityEngine;

public static class MissionLoader
{
    public static ChapterModel LoadChapter(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Mission", $"{fileName}.json");
        if (!File.Exists(filePath))
        {
            Debug.LogError($"Chapter file not found: {filePath}");
            return null;
        }

        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<ChapterModel>(json);
    }
}
