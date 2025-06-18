using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public event Action OnInventoryUpdated;

    private List<InventoryItemModel> inventoryItems = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(ItemSO itemSO)
    {
        var newItem = new InventoryItemModel(itemSO);
        inventoryItems.Add(newItem);
        OnInventoryUpdated?.Invoke();

        Debug.Log($"[InventoryManager] Added item: {itemSO.itemName}");

        SaveSystem.Instance?.SaveAll(); // Optional: auto-save
    }

    public void LoadItems(List<InventoryItemModel> loadedItems)
    {
        inventoryItems = loadedItems ?? new List<InventoryItemModel>();
        OnInventoryUpdated?.Invoke();
    }

    public List<InventoryItemModel> GetItems() => inventoryItems;

    public void ClearInventory()
    {
        inventoryItems.Clear();
        OnInventoryUpdated?.Invoke();
    }
}
