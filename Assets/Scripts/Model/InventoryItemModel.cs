using System;

[Serializable]
public class InventoryItemModel
{
    public ItemSO itemSO;

    public InventoryItemModel(ItemSO itemSO)
    {
        this.itemSO = itemSO;
    }
}
