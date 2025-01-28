using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{
    public float dirX, dirY;
    public int speed;
    public Joystick joystick;
    private Rigidbody2D rb;
    private bool facingRight = true;

    public Transform weaponTransform;   
    public LayerMask enemyLayer;        
    public float detectionRadius = 10f;

    public float maxHealth = 100f;
    public float currentHealth;

    public Vector3 initialPosition;
    public float initialHealth = 100f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Сохраняем начальные значения
        initialPosition = transform.position;
        currentHealth = initialHealth;

        LoadPlayerData();  // Загрузка данных, если есть сохранение
    }

    private void OnApplicationQuit()
    {
        SavePlayerData();
    }

    private void Update()
    {
        dirX = joystick.Horizontal * speed;
        dirY = joystick.Vertical * speed;
        rb.velocity = new Vector2(dirX, dirY);
        GameObject closestEnemy = FindClosestEnemy();
        if (closestEnemy != null)
        {
            FlipTowardsEnemy(closestEnemy.transform);
            AimWeaponAtEnemy(closestEnemy.transform);
        }
        else
        {
            FlipTowardsMovement();
        }
    }

    private void FlipTowardsEnemy(Transform enemy)
    {
        float enemyPositionX = enemy.position.x;
        if (facingRight && enemyPositionX < transform.position.x)
        {
            Flip();
        }
        else if (!facingRight && enemyPositionX > transform.position.x)
        {
            Flip();
        }
    }

    private void FlipTowardsMovement()
    {
        if (dirX > 0 && !facingRight)
        {
            Flip();
        }
        else if (dirX < 0 && facingRight)
        {
            Flip();
        }
    }

    private void AimWeaponAtEnemy(Transform enemy)
    {
        if (enemy == null) return;

        Vector3 direction = (enemy.position - weaponTransform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (!facingRight)
        {
            angle += 180f;
        }
        weaponTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public GameObject FindClosestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (Collider2D hit in hits)
        {
            float distance = Vector2.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = hit.gameObject;
            }
        }
        return closestEnemy;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    private void SavePlayerData()
    {
        SaveData data = new SaveData
        {
            playerHealth = currentHealth,
            playerPosition = new float[]
            {
            transform.position.x,
            transform.position.y,
            transform.position.z
            },
            ammoCount = GetComponent<Inventory>().ammoSlot.count
        };

        Inventory inventory = GetComponent<Inventory>();
        if (inventory != null)
        {
            foreach (var slot in inventory.slots)
            {
                if (!slot.isEmpty)
                {
                    data.inventorySlots.Add(new InventoryItemData { itemName = slot.item.itemName, count = slot.count });
                }
            }
        }

        SaveManager.Save(data);
    }

    private void LoadPlayerData()
    {
        SaveData data = SaveManager.Load();

        if (data != null)
        {
            currentHealth = data.playerHealth;
            transform.position = new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);

            Inventory inventory = GetComponent<Inventory>();
            AmmoSlot ammoSlot = FindObjectOfType<AmmoSlot>();

            inventory?.LoadInventory(data.inventorySlots);
            ammoSlot?.LoadAmmoSlot(data.ammoSlot);
        }
    }

    private ItemScriptableObject GetItemByName(string itemName)
    {
        return ItemDataBase.Instance.GetItemByName(itemName);
    }

    public void ResetToInitialValues()
    {
        // Сброс состояния игрока
        transform.position = initialPosition;
        currentHealth = initialHealth;

        // Сброс инвентаря
        Inventory inventory = GetComponent<Inventory>();
        if (inventory != null)
        {
            inventory.ResetInventory();
        }

        Debug.Log("Игрок и инвентарь сброшены к начальным значениям.");
    }
}
