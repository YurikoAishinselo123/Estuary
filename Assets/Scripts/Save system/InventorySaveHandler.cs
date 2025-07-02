using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InventorySaveHandler : ISaveableDataHandler
{
    private string savePath => Path.Combine(Application.persistentDataPath, "inventory.json");

    public void SaveData()
    {
        List<InventoryItemModel> items = InventoryManager.Instance.GetItems();
        List<InventoryItemSaveDataModel> saveDataList = new List<InventoryItemSaveDataModel>();

        foreach (var item in items)
        {
            saveDataList.Add(new InventoryItemSaveDataModel
            {
                itemID = item.itemSO.itemID,
                quantity = item.quantity
            });
        }

        InventorySaveWrapper wrapper = new InventorySaveWrapper { items = saveDataList };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
    }

    public void LoadData()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("[InventorySaveHandler] No inventory save file found.");
            return;
        }

        string json = File.ReadAllText(savePath);
        InventorySaveWrapper wrapper = JsonUtility.FromJson<InventorySaveWrapper>(json);

        InventoryManager.Instance.LoadFromSave(wrapper.items);
    }

    [System.Serializable]
    private class InventorySaveWrapper
    {
        public List<InventoryItemSaveDataModel> items;
    }
}
