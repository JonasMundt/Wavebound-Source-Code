using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 direction)
    {
        Vector2 moveDirection = direction.normalized;

        // Bewegung
        if (rb != null)
        {
            rb.velocity = moveDirection * speed;
        }

        // Lebensdauer
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(1f); // Projektil macht 1 Schaden
            }

            Destroy(gameObject); // Projektil zerstören
        }
    }
}
