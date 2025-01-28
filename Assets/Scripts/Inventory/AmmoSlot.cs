using UnityEngine;
using TMPro;

public class AmmoSlot : MonoBehaviour
{
    public ItemScriptableObject item;
    public int count;
    public TMP_Text itemCount;

    private void Awake()
    {
        itemCount = transform.GetChild(1).GetComponent<TMP_Text>();
    }

    public AmmoSlotData GetAmmoSlotData()
    {
        if (item == null) return null;

        return new AmmoSlotData
        {
            itemName = item.itemName,
            count = count
        };
    }

    public void LoadAmmoSlot(AmmoSlotData data)
    {
        if (data == null) return;

        item = ItemDataBase.Instance.GetItemByName(data.itemName);
        count = data.count;

        if (itemCount != null)
        {
            itemCount.text = count.ToString();
        }
    }
}
