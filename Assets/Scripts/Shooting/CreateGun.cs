using UnityEngine;

[CreateAssetMenu(fileName = "GunItems", menuName = "Inventory/Items/Create new gun")]
public class CreateGun : ItemScriptableObject
{
    public GameObject bullet;
    public Transform shotPoint;
    public float damage;
    public double timeBtwShoots;
    public double startTimeBtwShoots;
}
