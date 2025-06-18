using System.Collections.Generic;
using UnityEngine;

public static class InventorySaveHelper
{
    private const string InventoryKey = "InventoryItems";

    public static void Save(List<InventoryItemModel> items)
    {
        string json = JsonUtility.ToJson(new InventorySaveData(items));
        PlayerPrefs.SetString(InventoryKey, json);
        PlayerPrefs.Save();
    }

    public static List<InventoryItemModel> Load()
    {
        if (!PlayerPrefs.HasKey(InventoryKey)) return new List<InventoryItemModel>();

        string json = PlayerPrefs.GetString(InventoryKey);
        InventorySaveData data = JsonUtility.FromJson<InventorySaveData>(json);
        return data?.items ?? new List<InventoryItemModel>();
    }

    public static void ResetInventory()
    {
        PlayerPrefs.DeleteKey(InventoryKey);
    }

    [System.Serializable]
    private class InventorySaveData
    {
        public List<InventoryItemModel> items;

        public InventorySaveData(List<InventoryItemModel> items)
        {
            this.items = items;
        }
    }
}
