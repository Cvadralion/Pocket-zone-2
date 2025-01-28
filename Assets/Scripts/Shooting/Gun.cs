using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;      
    public Transform firePoint;          
    public float fireCooldown = 0f;    
    private float lastFireTime = 0f;
    public Item ammos;

    public CreateGun gun;
    public Inventory inventory;
    private Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>(); 
    }

    public void Shoot()
    {
        if (inventory.CheckItem(ammos))
        {
            if (Time.time < lastFireTime + fireCooldown) return;
            lastFireTime = Time.time;
            GameObject closestEnemy = player.FindClosestEnemy();
            if (closestEnemy != null)
            {
                inventory.UseAmmo(ammos);
                Vector3 direction = (closestEnemy.transform.position - firePoint.position).normalized;
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.damage = gun.damage;
                bulletScript.SetDirection(direction);
            }
        }
    }
}


