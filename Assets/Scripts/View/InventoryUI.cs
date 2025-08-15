using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotParent;
    private int maxSlotCount = 2;

    private List<ItemSlotUI> slotUIs = new();

    void Awake()
    {
        slotPrefab.SetActive(true);
    }

    private void OnEnable()
    {
        Debug.Log($"[InventoryUI] OnEnable called on {gameObject.name}");

        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStarted += HandleDialogueStarted;
            DialogueManager.Instance.OnDialogueEnded += HandleDialogueEnded;
        }

        CameraTool.OnPhotoCaptureStarted += HandlePhotoCaptureStarted;
        CameraTool.OnPhotoCaptured += HandlePhotoCaptured;

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryUpdated -= UpdateUI;
            InventoryManager.Instance.OnInventoryUpdated += UpdateUI;

            InventoryManager.Instance.OnItemSelected -= HighlightSelectedSlot;
            InventoryManager.Instance.OnItemSelected += HighlightSelectedSlot;

            UpdateUI(InventoryManager.Instance.GetItems());
        }
    }

    private void OnDisable()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnDialogueStarted -= HandleDialogueStarted;
            DialogueManager.Instance.OnDialogueEnded -= HandleDialogueEnded;
        }

        CameraTool.OnPhotoCaptureStarted -= HandlePhotoCaptureStarted;
        CameraTool.OnPhotoCaptured -= HandlePhotoCaptured;

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryUpdated -= UpdateUI;
            InventoryManager.Instance.OnItemSelected -= HighlightSelectedSlot;
        }
    }

    private void HandlePhotoCaptureStarted()
    {
        slotParent.gameObject.SetActive(false);
    }

    private void HandlePhotoCaptured(Texture2D photo)
    {
        StartCoroutine(ReenableInventoryWithDelay(0.2f));
    }

    private IEnumerator ReenableInventoryWithDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        slotParent.gameObject.SetActive(true);
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
                    Debug.Log($"[InventoryUI] Spawned item in slot {i}: {items[i].itemSO.itemName} (ID: {items[i].itemSO.itemID}, Quantity: {items[i].quantity})");
                }
                else
                {
                    slotUI.Clear();
                }

                slotUI.SetHighlight(i == selectedIndex);
            }
        }
    }

    private void HandleDialogueStarted(NPCDialogueModel model, NPCController speaker)
    {
        slotParent.gameObject.SetActive(false);
    }

    private void HandleDialogueEnded()
    {
        slotParent.gameObject.SetActive(true);
    }

    private void HighlightSelectedSlot(int selectedIndex)
    {
        for (int i = 0; i < slotUIs.Count; i++)
        {
            slotUIs[i].SetHighlight(i == selectedIndex);
        }
    }
}
