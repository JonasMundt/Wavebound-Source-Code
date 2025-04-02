using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 2f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (gameObject.name.Contains("Boss"))
    {
        FindObjectOfType<GameOverManager>()?.ShowEndScreen();
    }
        Destroy(gameObject);
    }
}
