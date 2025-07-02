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

        // 📦 Handle number key selection
        int keyIndex = InputManager.Instance.GetSelectedItemByKey();
        if (keyIndex != -1)
        {
            int inventoryIndex = keyIndex - 1;
            InventoryManager.Instance.SelectItem(inventoryIndex);
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
    }
}
