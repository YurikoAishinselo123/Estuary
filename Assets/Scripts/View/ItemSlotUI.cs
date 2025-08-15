using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private GameObject itemName;

    [Header("Colors")]
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color highlightColor = Color.green;
    [SerializeField] private float scaleDuration = 0.1f; //For ItemSlot Tween

    private Coroutine scaleCoroutine; 
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
            backgroundImage.color = isHighlighted ? highlightColor : defaultColor; // To change image color into highlight color - ganti warna bg gambar
        }

        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);

        Vector3 targetScale = isHighlighted ? Vector3.one * 1.2f : Vector3.one;
        scaleCoroutine = StartCoroutine(AnimateScale(targetScale));


        if (itemName != null)
        {
            itemName.SetActive(isHighlighted); // To make item name component appear and disappear - spy component itemname(nama item) muncul dan ilang
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

    private IEnumerator AnimateScale(Vector3 targetScale) 
    {
        Vector3 startScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed < scaleDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / scaleDuration;
            t = Mathf.SmoothStep(0f, 1f, t); 
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        transform.localScale = targetScale; 
    }

}

