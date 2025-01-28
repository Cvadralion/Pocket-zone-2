using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject UiPanel;
    public Transform inventory;
    public AmmoSlot ammoSlot;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public bool isOpened;

    public GameObject deleteButton;
    public InventorySlot selectedSlot; 

    private void Awake()
    {
        UiPanel.SetActive(true);
        UiPanel.SetActive(false);


        for (int i = 0; i < inventory.childCount; i++)
        {
            if (inventory.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                slots.Add(inventory.GetChild(i).GetComponent<InventorySlot>());
            }
        }
    }

    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            Item itemComponent = other.GetComponent<Item>();
            if (itemComponent != null)
            {
                AddItem(itemComponent.gameObject.GetComponent<Item>().itemScriptableObject, itemComponent.gameObject.GetComponent<Item>().count);
                if (itemComponent.gameObject.GetComponent<Item>().itemScriptableObject.itemType == ItemType.Ammo)
                {
                    ammoSlot.count += itemComponent.gameObject.GetComponent<Item>().count;
                    ammoSlot.itemCount.text = ammoSlot.count.ToString();
                }
                Destroy(other.gameObject);
            }
        }
    }

    public void OpenInventory()
    {
        if (isOpened == false)
        {
            isOpened = true;
            UiPanel.SetActive(true);
        }
        else if (isOpened == true)
        {
            isOpened = false;
            UiPanel.SetActive(false);
        }
    }

    public void AddItem(ItemScriptableObject _item, int _count)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == _item)
            {
                slot.count += _count;
                if (slot.count == 1)
                {
                    slot.itemCount.text = "";
                }
                else
                    slot.itemCount.text = slot.count.ToString();
                return;
            }
        }
        foreach (InventorySlot slot in slots)
        {
            if (slot.isEmpty == true)
            {
                slot.item = _item;
                slot.count = _count;
                slot.isEmpty = false;
                slot.Seticon(_item.icon);
                if (slot.count == 1)
                {
                    slot.itemCount.text = "";
                } 
                else
                    slot.itemCount.text = _count.ToString();
                return;
            }
        }
    }

    public void UseAmmo(Item item)
    {
        if (slots == null) return;

        for (int i = slots.Count - 1; i >= 0; i--)
        {
            if (slots[i]?.item != null && slots[i].item.itemName == item.itemScriptableObject.itemName)
            {
                if (item.itemScriptableObject.itemType == ItemType.Ammo)
                {
                    slots[i].count--;
                    slots[i].itemCount.text = slots[i].count.ToString();
                    ammoSlot.count--;
                    ammoSlot.itemCount.text = ammoSlot.count.ToString();
                }
                if (slots[i].count <= 0)
                {
                    slots[i].ClearSlot();
                }
                break;
            }
        }
    }

    public bool CheckItem(Item item)
    {
        if (slots == null) return false;

        for (int i = slots.Count - 1; i >= 0; i--)
        {
            if (slots[i]?.item != null && slots[i].item.itemName == item.itemScriptableObject.itemName)
            {
                return true;
            }
        }
        return false;
    }

    public void SelectSlot(InventorySlot _slot)
    {
        selectedSlot = _slot;
        if (selectedSlot.isEmpty)
        { 
            deleteButton.SetActive(false); 
        }
        else 
        {
            deleteButton.SetActive(true); 
        }
        
    }

    public void DeleteItem()
    {
        if (selectedSlot != null && !selectedSlot.isEmpty)
        {
            ItemScriptableObject itemData = selectedSlot.item;
            if (itemData.itemType == ItemType.Ammo)
            {
                ammoSlot.count -= selectedSlot.count;
                ammoSlot.itemCount.text = ammoSlot.count.ToString();
            }
            selectedSlot.ClearSlot();
            selectedSlot = null;
            deleteButton.SetActive(false);
        }
    }

    private ItemScriptableObject GetItemByName(string itemName)
    {
        foreach (var item in ItemDataBase.Instance.allItems)
        {
            if (item.itemName == itemName)
            {
                return item;
            }
        }
        return null;
    }
    public List<InventoryItemData> GetInventoryData()
    {
        List<InventoryItemData> inventoryData = new List<InventoryItemData>();

        foreach (InventorySlot slot in slots)
        {
            if (!slot.isEmpty)
            {
                InventoryItemData itemData = new InventoryItemData
                {
                    itemName = slot.item.itemName,
                    count = slot.count,
                    itemType = slot.item.itemType.ToString()
                };
            }
        }

        return inventoryData;
    }

    public void LoadInventory(List<InventoryItemData> savedItems)
    {
        foreach (InventoryItemData itemData in savedItems)
        {
            ItemScriptableObject item = ItemDataBase.Instance.GetItemByName(itemData.itemName);

            if (item != null)
            {
                AddItem(item, itemData.count);
                if (item.itemType == ItemType.Ammo)
                {
                    ammoSlot.count = itemData.count;
                    ammoSlot.itemCount.text = ammoSlot.count.ToString();
                }
            }
            else
            {
                Debug.LogWarning("Предмет не найден в базе данных: " + itemData.itemName);
            }
        }
    }


    public void ResetInventory()
    {
        foreach (InventorySlot slot in slots)
        {
            slot.ClearSlot();
        }

        ammoSlot.item = null;
        ammoSlot.count = 0;
        ammoSlot.itemCount.text = "0";

        selectedSlot = null;
        deleteButton.SetActive(false);
    }

    public void OnResetButtonClick()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.ResetToInitialValues();
        }
    }
}
