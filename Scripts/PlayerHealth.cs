using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public TMP_Text healthText;
    public Slider healthBar;
    private GameOverManager gameOverManager;
    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
            healthBar.maxValue = maxHealth;

        UpdateHealthBar();
        gameOverManager = FindObjectOfType<GameOverManager>();

    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

void UpdateHealthBar()
{
    if (healthBar != null)
    {
        healthBar.value = currentHealth;
    }

    if (healthText != null)
    {
        healthText.text = currentHealth + " / " + maxHealth;
    }
}
    void Die()
    {
        Debug.Log("Player died!");
        gameOverManager.ShowGameOver();
        gameObject.SetActive(false);
    }

    public void SetMaxHP(int newMax)
{
    maxHealth = newMax;
    currentHealth = maxHealth;

    if (healthBar != null)
        healthBar.maxValue = maxHealth;

    UpdateHealthBar();
}

}
