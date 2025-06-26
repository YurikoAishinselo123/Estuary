using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private void Update()
    {
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
        {
            if (InputManager.Instance.SpaceKey)
            {
                DialogueManager.Instance.ContinueDialogue();
            }
            return;
        }

        if (InputManager.Instance.Interact)
        {
            HandleInteraction();
        }

        if (InputManager.Instance.Action)
        {
            ToolManager.Instance.UseTool();
        }

        int selectedIndex = InputManager.Instance.GetSelectedItemByKey();
        if (selectedIndex != -1)
        {
            TrySelectToolByIndex(selectedIndex - 1);
        }
    }

    private void HandleInteraction()
    {
        var detected = DetectionManager.Instance?.GetCurrentDetected();
        if (detected == null) return;

        if (detected is CollectibleItem item)
        {
            InventoryManager.Instance.AddItem(item.itemData);
            Destroy(item.gameObject);
        }
        else if (detected is NPC npc)
        {
            npc.TriggerDialogue(); 
        }
        else
        {
            Debug.LogWarning("Detected object is not interactable.");
        }
    }

    private void TrySelectToolByIndex(int index)
    {
        var items = InventoryManager.Instance.GetItems();
        if (index >= 0 && index < items.Count)
        {
            var selectedItem = items[index];
            ToolManager.Instance.SelectTool(selectedItem.itemSO.itemType);
        }
        else
        {
            Debug.Log($"No item at index {index} in inventory.");
        }
    }
}
