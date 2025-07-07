using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public event Action<List<InventoryItemModel>> OnInventoryUpdated;
    public event Action<int> OnItemSelected;

    private List<InventoryItemModel> inventoryItems = new();
    private int selectedIndex = -1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional
    }

    public void AddItem(ItemSO itemSO)
    {
        if (itemSO == null)
        {
            Debug.LogWarning("[InventoryManager] Tried to add null ItemSO.");
            return;
        }

        var existing = inventoryItems.Find(i => i.itemSO == itemSO);
        if (existing != null)
        {
            existing.quantity++;
        }
        else
        {
            inventoryItems.Add(new InventoryItemModel(itemSO));
        }

        Debug.Log($"[InventoryManager] Item added: {itemSO.itemName}");

        OnInventoryUpdated?.Invoke(GetItems());
        MissionManager.Instance?.NotifyItemCollected(itemSO);
        SaveSystem.Instance?.SaveAll();
    }

    public void SelectItem(int index)
    {
        // 🔁 Toggle: selecting the same index = unselect
        if (index == selectedIndex)
        {
            selectedIndex = -1;
            OnItemSelected?.Invoke(selectedIndex);
            return;
        }

        if (index < 0 || index >= inventoryItems.Count)
        {
            Debug.LogWarning($"[InventoryManager] Invalid selection index: {index}");

            if (selectedIndex != -1)
            {
                selectedIndex = -1;
                OnItemSelected?.Invoke(selectedIndex); 
            }

            return;
        }

        selectedIndex = index;
        OnItemSelected?.Invoke(selectedIndex);
    }


    public int GetSelectedIndex()
    {
        return selectedIndex;
    }

    public List<InventoryItemModel> GetItems()
    {
        return new List<InventoryItemModel>(inventoryItems);
    }

    public void LoadFromSave(List<InventoryItemSaveDataModel> savedData)
    {
        inventoryItems.Clear();

        if (ItemDatabase.Instance == null)
        {
            Debug.LogError("[InventoryManager] ItemDatabase.Instance is null.");
            return;
        }

        foreach (var data in savedData)
        {
            var itemSO = ItemDatabase.Instance.GetItemByID(data.itemID);
            if (itemSO != null)
            {
                inventoryItems.Add(new InventoryItemModel(itemSO, data.quantity));
            }
        }

        Debug.Log($"[InventoryManager] Loaded {inventoryItems.Count} items from save.");
        OnInventoryUpdated?.Invoke(GetItems());
    }

    public void ClearInventory()
    {
        inventoryItems.Clear();
        Debug.Log("[InventoryManager] Inventory cleared.");
        OnInventoryUpdated?.Invoke(GetItems());
    }
}
