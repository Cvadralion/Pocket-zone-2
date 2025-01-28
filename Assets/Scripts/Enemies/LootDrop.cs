using UnityEngine;

public class LootDrop : MonoBehaviour
{
    public Item[] lootItems; 
    public float dropChance = 1f; 

    public void DropLoot()
    {
        // Проверяем шанс выпадения
        if (Random.value <= dropChance && lootItems.Length > 0)
        {
            // Выбираем случайный предмет из массива
            int randomIndex = Random.Range(0, lootItems.Length);
            Item lootItem = lootItems[randomIndex];

            // Спавним предмет на позиции врага
            Instantiate(lootItem, transform.position, Quaternion.identity);
        }
    }
}
