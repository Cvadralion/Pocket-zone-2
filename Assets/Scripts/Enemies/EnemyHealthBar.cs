using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Image healthBar;
    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>(); // —сылаемс€ на компонент врага
    }

    private void OnTriggerEnter2D(Collider2D bullet)
    {
        if (bullet.CompareTag("Bullet"))
        {
            healthBar.fillAmount = enemy.currentHealth / enemy.maxHealth;
        }
    }
}
