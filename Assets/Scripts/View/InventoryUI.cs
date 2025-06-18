using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform itemSlotParent; // Parent layout container
    [SerializeField] private GameObject itemSlotPrefab; // Prefab to spawn

    private void OnEnable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryUpdated += UpdateUI;
        }
    }

    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryUpdated -= UpdateUI;
        }
    }

    public void UpdateUI()
    {
        if (itemSlotParent == null || itemSlotPrefab == null)
        {
            Debug.LogError("[InventoryUI] Missing UI references.");
            return;
        }

        // Clear existing slots
        foreach (Transform child in itemSlotParent)
        {
            Destroy(child.gameObject);
        }

        // Spawn a slot for each collected item
        List<InventoryItemModel> items = InventoryManager.Instance.GetItems();

        foreach (var item in items)
        {
            GameObject slotGO = Instantiate(itemSlotPrefab, itemSlotParent);
            ItemSlotUI slotUI = slotGO.GetComponent<ItemSlotUI>();
            slotUI.Setup(item.itemSO);
        }
    }
}
