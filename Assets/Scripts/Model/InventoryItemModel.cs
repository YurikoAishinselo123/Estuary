[System.Serializable]
public class InventoryItemModel
{
    public ItemSO itemSO;
    public int quantity;

    public InventoryItemModel(ItemSO itemSO, int quantity = 1)
    {
        this.itemSO = itemSO;
        this.quantity = quantity;
    }
}
