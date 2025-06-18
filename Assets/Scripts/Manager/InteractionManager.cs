using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    private void Update()
    {
        if (InputManager.Instance.Interact)
        {
            TryCollectItem();
        }
    }

    private void TryCollectItem()
    {
        var detectedItem = DetectionManager.Instance.GetCurrentDetectedItem();
        if (detectedItem != null)
        {
            InventoryManager.Instance.AddItem(detectedItem.itemData);
            Destroy(detectedItem.gameObject);
        }
    }
}
