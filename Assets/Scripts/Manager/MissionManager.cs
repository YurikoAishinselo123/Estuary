using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }

    public event Action<ChapterModel> OnChapterLoaded;
    public ChapterModel CurrentChapter { get; private set; }
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
        }
        else
        {
            Debug.Log("[MissionManager] All missions in chapter completed.");
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

        // If all missions completed in this chapter, advance
        bool allMissionsDone = CurrentChapter.missions.All(m => MissionSaveHelper.IsMissionCompleted(m.id));

        GameProgressManager.Instance?.UpdateFromMissionProgress();

        if (allMissionsDone)
        {
            Debug.Log("[MissionManager] All missions complete. Advancing to next chapter...");
            CompleteCurrentChapterAndAdvance();
        }
        else
        {
            // ✅ Reload current chapter before reloading mission
            // LoadChapter($"Chapter1");
        }
    }


    public void NotifyItemCollected(ItemSO itemSO)
    {
        if (itemSO == null || currentMission == null) return;

        // Mission 1: Collect camera and vacuum
        if (currentMission.id == 1)
        {
            if (itemSO.itemID == "camera" || itemSO.itemID == "vacuum")
            {
                if (collectedItemIDs.Add(itemSO.itemID)) // Only log new items
                {
                    Debug.Log($"[MissionManager] Collected tool: {itemSO.itemID}");
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
}
