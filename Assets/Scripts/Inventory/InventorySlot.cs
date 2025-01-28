using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public ItemScriptableObject item;
    public int count;
    public bool isEmpty = true;
    public GameObject iconGO;
    public TMP_Text itemCount;

    private void Awake()
    {
        iconGO = transform.GetChild(0).gameObject;
        itemCount = transform.GetChild(1).GetComponent<TMP_Text>();
    }

    public void Seticon(Sprite icon)
    {
        iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        iconGO.GetComponent<Image>().sprite = icon;
    }

    public void ClearSlot()
    {
        item = null;
        count = 0;
        isEmpty = true;
        iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        iconGO.GetComponent<Image>().sprite = null;
        itemCount.text = string.Empty;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        inventory.SelectSlot(this);
    }
}
