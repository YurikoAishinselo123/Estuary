using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GuidanceManager : MonoBehaviour
{
    public static GuidanceManager Instance { get; private set; }
    private const string GUIDANCE_INDEX_KEY = "GuidanceIndex";


    public event Action<string> OnGuidanceChanged;

    private List<string> guidanceList;
    private int currentIndex = -1;

    [SerializeField] private string fileName = "guidance.json";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        guidanceList = new List<string>(); 
        LoadGuidanceFromFile();
    }


    private void Start()
    {
        // Debug.Log("[GuidanceManager] Starting with index: " + currentIndex);
        // Debug.Log("[GuidanceManager] Guidance count: " + guidanceList?.Count);

        LoadSavedIndex(); 

        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.OnPlayerStartedMoving += HandlePlayerStartedMoving;
            player.OnPlayerJumped += HandlePlayerJumped;
        }

        ShowNextGuidance(); // Show saved message or first one
    }



    private void LoadGuidanceFromFile()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Guidance", fileName);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Debug.Log($"[GuidanceManager] Loading guidance from path: {path}");

            GuidanceDataModel data = JsonUtility.FromJson<GuidanceDataModel>(json);

            if (data == null || data.guidance == null || data.guidance.Count == 0)
            {
                Debug.LogWarning("[GuidanceManager] JSON loaded but no guidance found.");
            }
            else
            {
                guidanceList = new List<string>(data.guidance);
                Debug.Log($"[GuidanceManager] Loaded {guidanceList.Count} guidance entries.");
            }
        }
        else
        {
            Debug.LogError("[GuidanceManager] File not found at path: " + path);
            guidanceList = new List<string>();
        }
    }


    private void HandlePlayerStartedMoving()
    {
        if (currentIndex == 0)
            ShowNextGuidance();
    }

    private void HandlePlayerJumped()
    {
        if (currentIndex == 1)
            ShowNextGuidance();
    }

    public bool HasNext()
    {
        return currentIndex + 1 < guidanceList.Count;
    }

    public void ShowNextGuidance()
    {
        if (guidanceList == null || guidanceList.Count == 0)
        {
            Debug.LogWarning("[GuidanceManager] No guidance to show (list is null or empty).");
            return;
        }

        currentIndex++;
        SaveCurrentIndex();
        if (currentIndex >= 0 && currentIndex < guidanceList.Count)
        {
            string text = guidanceList[currentIndex];
            Debug.Log($"[GuidanceManager] Showing guidance #{currentIndex}: {text}");
            OnGuidanceChanged?.Invoke(text);
        }
        else
        {
            Debug.Log("[GuidanceManager] No more guidance to show.");
        }
    }

    private void SaveCurrentIndex()
    {
        PlayerPrefs.SetInt(GUIDANCE_INDEX_KEY, currentIndex);
        PlayerPrefs.Save();
        Debug.Log("[GuidanceManager] Saved guidance index: " + currentIndex);
    }

    private void LoadSavedIndex()
    {
        currentIndex = PlayerPrefs.GetInt(GUIDANCE_INDEX_KEY, -1);
        Debug.Log("[GuidanceManager] Loaded saved index: " + currentIndex);
    }


    public void ResetGuidance()
    {
        currentIndex = -1;
        PlayerPrefs.DeleteKey(GUIDANCE_INDEX_KEY);
        PlayerPrefs.Save();
        Debug.Log("[GuidanceManager] Guidance reset.");
    }

}
