using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotParent;
    [SerializeField] private int maxSlotCount = 4;

    private List<ItemSlotUI> slotUIs = new();

    private void OnEnable()
    {
        Debug.Log($"[InventoryUI] OnEnable called on {gameObject.name}");
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryUpdated -= UpdateUI;
            InventoryManager.Instance.OnInventoryUpdated += UpdateUI;

            InventoryManager.Instance.OnItemSelected -= HighlightSelectedSlot;
            InventoryManager.Instance.OnItemSelected += HighlightSelectedSlot;

            // Initial display
            // UpdateUI(InventoryManager.Instance.GetItems());

            var items = InventoryManager.Instance.GetItems();
            UpdateUI(items);
        }
    }

    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryUpdated -= UpdateUI;
            InventoryManager.Instance.OnItemSelected -= HighlightSelectedSlot;
        }
    }

    private void UpdateUI(List<InventoryItemModel> items)
    {
        // Destroy old slot UI objects
        foreach (var slot in slotUIs)
        {
            if (slot != null)
                Destroy(slot.gameObject);
        }
        slotUIs.Clear();

        int selectedIndex = InventoryManager.Instance.GetSelectedIndex();

        for (int i = 0; i < maxSlotCount; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotParent);
            ItemSlotUI slotUI = slotObj.GetComponent<ItemSlotUI>();

            if (slotUI != null)
            {
                slotUIs.Add(slotUI);

                if (i < items.Count)
                {
                    slotUI.Setup(items[i].itemSO);
                }
                else
                {
                    slotUI.Clear(); // You should implement this method in ItemSlotUI
                }

                slotUI.SetHighlight(i == selectedIndex);
            }
        }
    }

    private void HighlightSelectedSlot(int selectedIndex)
    {
        for (int i = 0; i < slotUIs.Count; i++)
        {
            slotUIs[i].SetHighlight(i == selectedIndex);
        }
    }
}
