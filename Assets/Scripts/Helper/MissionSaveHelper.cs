using UnityEngine;

/// <summary>
/// Handles mission progress saving and loading using PlayerPrefs.
/// </summary>
public static class MissionSaveHelper
{
    /// <summary>
    /// Check if a mission is marked as completed.
    /// </summary>
    public static bool IsMissionCompleted(int missionId)
    {
        return PlayerPrefs.GetInt($"Mission_{missionId}_Complete", 0) == 1;
    }

    /// <summary>
    /// Mark a mission as completed.
    /// </summary>
    public static void MarkMissionCompleted(int missionId)
    {
        PlayerPrefs.SetInt($"Mission_{missionId}_Complete", 1);
        PlayerPrefs.Save();
        Debug.Log($"[MissionSaveHelper] Mission {missionId} marked as complete.");
    }

    /// <summary>
    /// Check if all missions in the current chapter are completed.
    /// </summary>
    public static bool IsAllChapterMissionCompleted()
    {
        if (MissionManager.Instance?.CurrentChapter == null)
        {
            Debug.LogWarning("[MissionSaveHelper] CurrentChapter is null.");
            return false;
        }

        foreach (var mission in MissionManager.Instance.CurrentChapter.missions)
        {
            if (!IsMissionCompleted(mission.id))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Get the next incomplete mission ID in the chapter.
    /// </summary>
    public static int GetNextIncompleteMissionID(ChapterModel chapter)
    {
        if (chapter == null)
        {
            Debug.LogError("[MissionSaveHelper] Chapter is null.");
            return -1;
        }

        foreach (var mission in chapter.missions)
        {
            if (!IsMissionCompleted(mission.id))
                return mission.id;
        }

        return -1; // All missions completed
    }

    /// <summary>
    /// Clear all saved mission progress (for debugging or resetting).
    /// </summary>
    public static void ClearAllMissionProgress()
    {
        if (MissionManager.Instance?.CurrentChapter == null)
        {
            Debug.LogWarning("[MissionSaveHelper] Cannot clear missions, chapter is null.");
            return;
        }

        foreach (var mission in MissionManager.Instance.CurrentChapter.missions)
        {
            PlayerPrefs.DeleteKey($"Mission_{mission.id}_Complete");
            Debug.Log("delete mission progress");
        }

        PlayerPrefs.Save();
        Debug.Log("[MissionSaveHelper] All mission progress cleared.");
    }

    
}
