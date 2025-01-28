using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public int enemyCount = 3;     
    public PolygonCollider2D spawnArea;

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector2 randomPosition = GetRandomPosition();
            Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        }
    }

    Vector2 GetRandomPosition()
    {
        Bounds bounds = spawnArea.bounds;

        // Генерируем случайные координаты в пределах области
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(randomX, randomY);
    }
}