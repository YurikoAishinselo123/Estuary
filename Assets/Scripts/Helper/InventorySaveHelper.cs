using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class InventorySaveHelper
{
    private static string savePath => Path.Combine(Application.persistentDataPath, "inventory.json");

    [System.Serializable]
    private class InventorySaveWrapper
    {
        public List<InventoryItemSaveDataModel> items;
    }

    public static void Save(List<InventoryItemModel> inventoryItems)
    {
        var saveDataList = new List<InventoryItemSaveDataModel>();

        foreach (var model in inventoryItems)
        {
            saveDataList.Add(new InventoryItemSaveDataModel
            {
                itemID = model.itemSO.itemID,
                quantity = model.quantity
            });
        }

        InventorySaveWrapper wrapper = new InventorySaveWrapper { items = saveDataList };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
    }

    public static List<InventoryItemSaveDataModel> Load()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("[InventorySaveHelper] No inventory save file found.");
            return new List<InventoryItemSaveDataModel>();
        }

        string json = File.ReadAllText(savePath);
        InventorySaveWrapper wrapper = JsonUtility.FromJson<InventorySaveWrapper>(json);
        return wrapper.items ?? new List<InventoryItemSaveDataModel>();
    }

    public static void ResetInventory()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
    }
}
