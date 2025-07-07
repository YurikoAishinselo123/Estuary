using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }

    public event Action<ChapterModel> OnChapterLoaded;
    public event Action<MissionModel> OnMissionUpdated;
    public event Action<int> OnMissionProgressUpdated;

    public ChapterModel CurrentChapter { get; private set; }
    public MissionModel CurrentMission => currentMission;

    private MissionModel currentMission;
    private HashSet<string> collectedItemIDs = new();

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
        int currentChapterIndex = ChapterSaveHelper.GetCurrentChapter();
        Debug.Log("current chapter : " + currentChapterIndex);
        LoadChapter($"Chapter{currentChapterIndex}");
    }

    private void LoadChapter(string fileName)
    {
        CurrentChapter = MissionLoader.LoadChapter(fileName);
        Debug.Log("chapter : " + fileName);

        if (CurrentChapter != null)
        {
            Debug.Log($"[MissionManager] Loaded: {CurrentChapter.chapterTitle}");
            OnChapterLoaded?.Invoke(CurrentChapter);

            LoadCurrentMission();
        }
        else
        {
            Debug.LogError($"[MissionManager] Failed to load chapter: {fileName}");
        }
    }

    private void LoadCurrentMission()
    {
        int nextMissionId = MissionSaveHelper.GetNextIncompleteMissionID(CurrentChapter);
        currentMission = CurrentChapter.missions.Find(m => m.id == nextMissionId);

        if (currentMission != null)
        {
            Debug.Log($"[MissionManager] Current Mission: {currentMission.title}");
            OnMissionUpdated?.Invoke(currentMission);
        }
        else
        {
            Debug.Log("[MissionManager] All missions in chapter completed.");
            OnMissionUpdated?.Invoke(null); // Hide UI if needed
        }
    }

    public void CompleteCurrentMission()
    {
        if (currentMission == null)
        {
            Debug.LogWarning("[MissionManager] No active mission to complete.");
            return;
        }

        Debug.Log($"[MissionManager] Mission completed: {currentMission.title}");

        MissionSaveHelper.MarkMissionCompleted(currentMission.id);

        bool allMissionsDone = CurrentChapter.missions.All(m => MissionSaveHelper.IsMissionCompleted(m.id));
        GameProgressManager.Instance?.UpdateFromMissionProgress();

        if (allMissionsDone)
        {
            Debug.Log("[MissionManager] All missions complete. Advancing to next chapter...");
            // CompleteCurrentChapterAndAdvance();
        }
        else
        {
            LoadCurrentMission(); // Load next mission and notify UI
        }
    }

    public void CompleteCurrentChapterAndAdvance()
    {
        int currentChapter = ChapterSaveHelper.GetCurrentChapter();
        int nextChapter = currentChapter + 1;

        ChapterSaveHelper.SetCurrentChapter(nextChapter);
        // LoadChapter($"Chapter{nextChapter}");
    }

    public int GetCurrentChapterNumber()
    {
        return CurrentChapter != null ? CurrentChapter.chapterId : 1;
    }

    public void NotifyItemCollected(ItemSO itemSO)
    {
        if (itemSO == null || currentMission == null) return;

        // Mission 1: Collect camera and vacuum
        if (currentMission.id == 1)
        {
            if (itemSO.itemID == "camera" || itemSO.itemID == "vacuum")
            {
                if (collectedItemIDs.Add(itemSO.itemID)) // Only new items
                {
                    Debug.Log($"[MissionManager] Collected tool: {itemSO.itemID}");
                    OnMissionProgressUpdated?.Invoke(collectedItemIDs.Count);
                    // OnMissionUpdated?.Invoke(currentMission); // Optional: refresh UI

                }

                CheckMission1Completion();
            }
        }
    }

    private void CheckMission1Completion()
    {
        string[] requiredToolIDs = { "camera", "vacuum" };
        bool allCollected = requiredToolIDs.All(id => collectedItemIDs.Contains(id));

        if (allCollected)
        {
            CompleteCurrentMission();
        }
    }

    // Optional: call this from gameplay systems (e.g. camera/photo/vacuum)
    public void ReportMissionProgress(int currentAmount)
    {
        if (currentMission == null) return;
        OnMissionUpdated?.Invoke(currentMission);
    }
}
