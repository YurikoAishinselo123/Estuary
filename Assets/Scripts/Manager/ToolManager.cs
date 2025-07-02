using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public static ToolManager Instance { get; private set; }

    [SerializeField] private CameraTool cameraTool;
    [SerializeField] private VacuumTool vacuumTool;

    private ItemType? currentTool;

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

        if (index == -1)
        {
            Debug.Log("[ToolManager] Tool deselected.");
            currentTool = null;
            return;
        }

        if (index >= 0 && index < items.Count)
        {
            var item = items[index];
            currentTool = item.itemSO.itemType;
            Debug.Log($"[ToolManager] Selected tool: {currentTool}");
        }
        else
        {
            Debug.LogWarning($"[ToolManager] Invalid item index: {index}");
            currentTool = null;
        }
    }

    public void UseTool()
    {
        if (currentTool == null)
        {
            Debug.LogWarning("[ToolManager] No tool selected.");
            return;
        }

        switch (currentTool)
        {
            case ItemType.Camera:
                cameraTool?.Capture();
                break;
            case ItemType.Vacuum:
                vacuumTool?.Suck();
                break;
            default:
                Debug.LogWarning($"[ToolManager] Tool not handled: {currentTool}");
                break;
        }
    }
}
