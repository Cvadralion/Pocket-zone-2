using UnityEngine;

[CreateAssetMenu(fileName = "AmmoItems", menuName = "Inventory/Items/Create new ammo")]
public class CreateAmmo : ItemScriptableObject
{
    public float speed;
    public float lifeTime;
    public float distance;
    public LayerMask whatIsSolid;
}
