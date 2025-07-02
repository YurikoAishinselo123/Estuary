using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private Image backgroundImage;

    [Header("Colors")]
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color highlightColor = Color.green;

    private ItemSO currentItem;

    public void Setup(ItemSO itemSO)
    {
        currentItem = itemSO;

        if (itemIcon != null)
        {
            itemIcon.sprite = itemSO.itemIcon;
            itemIcon.enabled = itemSO.itemIcon != null;
        }

        if (itemNameText != null)
        {
            itemNameText.text = itemSO.itemName;
        }

        SetHighlight(false);

        Debug.Log($"[ItemSlotUI] Setup item: {itemSO.itemName}");
    }

    public void SetHighlight(bool isHighlighted)
    {
        if (backgroundImage != null)
        {
            backgroundImage.color = isHighlighted ? highlightColor : defaultColor;
        }
    }

    public ItemSO GetItem()
    {
        return currentItem;
    }

    public void Clear()
    {
        if (itemIcon != null)
        {
            itemIcon.sprite = null;
            itemIcon.enabled = false;
        }

        if (itemNameText != null)
        {
            itemNameText.text = "";
        }

        currentItem = null;
        SetHighlight(false);
    }


}
