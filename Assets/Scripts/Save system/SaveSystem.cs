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
        var loadedItems = InventorySaveHelper.Load();
        InventoryManager.Instance?.LoadItems(loadedItems);

        int chapter = ChapterSaveHelper.GetCurrentChapter();
        MissionManager.Instance?.CompleteCurrentChapterAndAdvance();

        Debug.Log("[SaveSystem] All data loaded.");
    }

    public void ResetAll()
    {
        InventorySaveHelper.ResetInventory();
        ChapterSaveHelper.ResetChapterProgress();
        PlayerPrefs.Save();
        Debug.Log("[SaveSystem] All data reset.");
    }
}
