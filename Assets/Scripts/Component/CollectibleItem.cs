using UnityEngine;

public class CollectibleItem : MonoBehaviour, IDetectable
{
    [Header("Item Configuration")]
    public ItemSO itemData;

    public string GetDisplayName()
    {
        if (itemData == null)
        {
            Debug.LogWarning($"CollectibleItem '{name}' has no assigned ItemSO!");
            return name;
        }

        return itemData.itemName;
    }

    // Optional: You can also expose item icon or prefab if needed
    public Sprite GetItemIcon() => itemData?.itemIcon;
    public GameObject GetItemPrefab() => itemData?.itemPrefab3D;
}
