using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GarbageFactEntry
{
    public string type;
    public string fact;
}

public class GarbageFactDatabase : MonoBehaviour
{
    public static GarbageFactDatabase Instance { get; private set; }

    [SerializeField] private string jsonFileName = "garbage_facts"; // filename without extension
    private Dictionary<string, string> factDict = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        LoadFactsFromJson();
    }

    private void LoadFactsFromJson()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Garbage", $"{jsonFileName}.json");

        if (!File.Exists(path))
        {
            Debug.LogError($"Garbage facts file not found at: {path}");
            return;
        }

        string json = File.ReadAllText(path);
        GarbageFactListModel factList = JsonUtility.FromJson<GarbageFactListModel>("{\"facts\":" + json + "}");

        factDict.Clear();
        foreach (var entry in factList.facts)
        {
            factDict[entry.type] = entry.fact;
        }
    }

    public string GetFact(GarbageType type)
    {
        return factDict.TryGetValue(type.ToString(), out string fact) ? fact : "Fakta tidak ditemukan.";
    }
}
