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
            if (model.itemSO == null)
            {
                Debug.LogWarning("[InventorySaveHelper] Skipped saving item with null ItemSO.");
                continue;
            }

            saveDataList.Add(new InventoryItemSaveDataModel
            {
                itemID = model.itemSO.itemID,
                quantity = model.quantity
            });

            Debug.Log($"[InventorySaveHelper] Saving item: {model.itemSO.itemName} (ID: {model.itemSO.itemID}, Qty: {model.quantity})");
        }

        InventorySaveWrapper wrapper = new InventorySaveWrapper { items = saveDataList };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"[InventorySaveHelper] Inventory saved to {savePath}");
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

        if (wrapper?.items != null)
        {
            foreach (var data in wrapper.items)
            {
                Debug.Log($"[InventorySaveHelper] Loaded item ID: {data.itemID}, Qty: {data.quantity}");
            }
        }

        return wrapper?.items ?? new List<InventoryItemSaveDataModel>();
    }

    public static void ResetInventory()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("[InventorySaveHelper] Inventory reset (file deleted).");
        }
    }
}
