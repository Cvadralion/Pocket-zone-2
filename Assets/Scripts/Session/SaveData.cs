using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public float playerHealth;
    public float[] playerPosition;
    public AmmoSlotData ammoSlot;
    public int ammoCount;
    public List<InventoryItemData> inventorySlots = new List<InventoryItemData>();
}

[System.Serializable]
public class SlotData
{
    public string itemName;
    public int count;
}