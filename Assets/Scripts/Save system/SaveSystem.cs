using UnityEngine;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SaveAll()
    {
        if (InventoryManager.Instance != null)
        {
            var items = InventoryManager.Instance.GetItems();
            InventorySaveHelper.Save(items);
            Debug.Log("Saved items : " + items);
        }

        if (MissionManager.Instance != null)
        {
            int chapter = MissionManager.Instance.GetCurrentChapterNumber();
            ChapterSaveHelper.SetCurrentChapter(chapter);
        }

        Debug.Log("[SaveSystem] All data saved.");
    }

    public void LoadAll()
    {
        var loadedSaveData = InventorySaveHelper.Load(); 
        Debug.Log("inventory data : " + loadedSaveData);
        InventoryManager.Instance?.LoadFromSave(loadedSaveData); 

        int chapter = ChapterSaveHelper.GetCurrentChapter();
        MissionManager.Instance?.CompleteCurrentChapterAndAdvance();

        Debug.Log("[SaveSystem] All data loaded.");
    }


    public void ResetAll()
    {
        InventorySaveHelper.ResetInventory();
        ChapterSaveHelper.ResetChapterProgress();
        MissionSaveHelper.ClearAllMissionProgress();
        GameProgressManager.Instance?.ResetProgress();
        PhotoSaveHelper.DeleteAllPhotos();
        PlayerPrefs.Save();
        Debug.Log("[SaveSystem] All data reset.");
    }
}
