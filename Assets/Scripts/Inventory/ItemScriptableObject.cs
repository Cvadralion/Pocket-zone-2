using UnityEngine;

public enum ItemType { Ammo, Weapon, Armor }
public class ItemScriptableObject : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public int maxCount;    
    public Sprite icon;
    public string itemDescription;
}
