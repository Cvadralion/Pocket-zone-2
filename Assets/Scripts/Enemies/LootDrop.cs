using UnityEngine;

public class LootDrop : MonoBehaviour
{
    public Item[] lootItems; 
    public float dropChance = 1f; 

    public void DropLoot()
    {
        // ��������� ���� ���������
        if (Random.value <= dropChance && lootItems.Length > 0)
        {
            // �������� ��������� ������� �� �������
            int randomIndex = Random.Range(0, lootItems.Length);
            Item lootItem = lootItems[randomIndex];

            // ������� ������� �� ������� �����
            Instantiate(lootItem, transform.position, Quaternion.identity);
        }
    }
}
