using UnityEngine;
using System.Collections.Generic;

public class ToolManager : MonoBehaviour
{
    public static ToolManager Instance { get; private set; }

    [SerializeField] private CameraTool cameraTool;
    [SerializeField] private VacuumTool vacuumTool;

    private ITool currentTool;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        InventoryManager.Instance.OnItemSelected += HandleItemSelected;
    }

    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnItemSelected -= HandleItemSelected;
    }

    private void HandleItemSelected(int index)
    {
        var items = InventoryManager.Instance.GetItems();

        currentTool?.Deactivate();
        currentTool = null;

        if (index == -1)
        {
            Debug.Log("[ToolManager] Tool deselected.");
            return;
        }

        if (index >= 0 && index < items.Count)
        {
            var item = items[index];
            switch (item.itemSO.itemType)
            {
                case ItemType.Camera:
                    currentTool = cameraTool;
                    break;
                case ItemType.Vacuum:
                    currentTool = vacuumTool;
                    break;
                default:
                    Debug.LogWarning($"[ToolManager] Tool not handled: {item.itemSO.itemType}");
                    break;
            }

            currentTool?.Activate();
            Debug.Log("Current Tool : " + currentTool);

            Debug.Log($"[ToolManager] Selected tool: {item.itemSO.itemType}");
        }
        else
        {
            Debug.LogWarning($"[ToolManager] Invalid item index: {index}");
        }
    }

    public void UseTool()
    {
        if (currentTool == null)
        {
            Debug.LogWarning("[ToolManager] No tool selected.");
            return;
        }

        currentTool.Use();
    }
}
