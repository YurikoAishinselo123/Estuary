using UnityEngine;
using UnityEngine.UI;
using TMPro; // This is the correct using directive

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;

    public void Setup(ItemSO item)
    {
        icon.sprite = item.itemIcon;
        nameText.text = item.itemName;
    }
}
