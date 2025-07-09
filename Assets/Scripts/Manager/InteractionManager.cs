using UnityEngine;
using UnityEngine.SceneManagement;

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

        bool isGameplay = GameStateManager.Instance.CurrentState == GameState.Gameplay;

        if (isGameplay && InputManager.Instance.Interact)
        {
            HandleInteraction();
        }

        if (isGameplay && InputManager.Instance.Action)
        {
            ToolManager.Instance.UseTool();
        }

        if (isGameplay)
        {
            int keyIndex = InputManager.Instance.GetSelectedItemByKey();
            if (keyIndex != -1)
            {
                int inventoryIndex = keyIndex - 1;
                InventoryManager.Instance.SelectItem(inventoryIndex);
            }
        }

        if (InputManager.Instance.Back)
        {
            // If photo UI is open, close it
            if (PhotoDisplayUI.Instance != null && PhotoDisplayUI.Instance.IsVisible())
            {
                PhotoDisplayUI.Instance.HidePhoto();

                // Only activate camera tool if returning to gameplay
                if (isGameplay)
                {
                    CameraTool.Instance.Activate();
                }

                return;
            }

            // Pause is allowed only in gameplay
            if (isGameplay && PauseUI.Instance != null)
            {
                if (PauseUI.Instance.isPaused)
                    PauseUI.Instance.ResumeGame();
                else
                    PauseUI.Instance.PauseGame();
                return;
            }
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
        else if (detected is Door door)
        {
            SpawnManager.LastSceneEnteredFrom = SceneManager.GetActiveScene().name.ToEnum<SceneName>();
            LoadSceneManager.Instance.LoadScene(door.GetTargetScene());
            
        }
    }
}
