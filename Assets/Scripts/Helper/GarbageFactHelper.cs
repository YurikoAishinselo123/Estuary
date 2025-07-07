using System.Collections.Generic;
using UnityEngine;
using System;

public static class GarbageFactHelper
{
    private static Dictionary<GarbageType, string> factDictionary;
    private static bool isInitialized = false;

    private const string ResourcePath = "json/garbage_facts"; // Resources/json/garbage_facts.json

    public static void Initialize()
    {
        if (isInitialized) return;

        TextAsset jsonText = Resources.Load<TextAsset>(ResourcePath);
        if (jsonText == null)
        {
            Debug.LogError($"[GarbageFactHelper] Could not find JSON at Resources/{ResourcePath}.json");
            return;
        }

        try
        {
            GarbageFactListWrapper data = JsonUtility.FromJson<GarbageFactListWrapper>(jsonText.text);
            factDictionary = new Dictionary<GarbageType, string>();

            foreach (var entry in data.facts)
            {
                try
                {
                    GarbageType type = entry.type.ToEnum<GarbageType>(ignoreCase: true);
                    factDictionary[type] = entry.fact;
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"[GarbageFactHelper] Skipping invalid enum '{entry.type}': {e.Message}");
                }
            }

            isInitialized = true;
            Debug.Log("[GarbageFactHelper] Loaded garbage facts successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[GarbageFactHelper] Failed to parse garbage facts JSON: {ex.Message}");
        }
    }

    public static string GetFact(GarbageType type)
    {
        if (!isInitialized)
            Initialize();

        if (factDictionary != null && factDictionary.TryGetValue(type, out var fact))
            return fact;

        return "Fakta tidak tersedia untuk sampah ini.";
    }
}
