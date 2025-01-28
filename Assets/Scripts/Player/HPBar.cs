using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HPBar : MonoBehaviour
{
    public Image bar;
    public Player player;

    private List<Enemy> enemiesInRange = new List<Enemy>();

    public GameObject deathPanel;
    public Button restartButton;

    private void Start()
    {
        player.currentHealth = player.maxHealth;
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
    }

    private void Update()
    {
        foreach (Enemy enemy in enemiesInRange)
        {
            if (enemy == null) continue;

            if (Time.time >= enemy.lastAttackTime + enemy.attackCooldown)
            {
                player.currentHealth -= enemy.damage;
                bar.fillAmount = player.currentHealth / player.maxHealth;

                enemy.lastAttackTime = Time.time;

                if (player.currentHealth <= 0)
                {
                    PlayerDeath();
                    break;
                }
            }
        }
    }

    private void PlayerDeath()
    {
        Time.timeScale = 0;
        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemyComponent = other.GetComponent<Enemy>();
            if (enemyComponent != null && !enemiesInRange.Contains(enemyComponent))
            {
                enemiesInRange.Add(enemyComponent);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemyComponent = other.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemiesInRange.Remove(enemyComponent);
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
