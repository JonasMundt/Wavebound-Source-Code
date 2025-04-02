using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float shootInterval = 0.5f;
    private float shootTimer = 0f;

    void Update()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval)
        {
            Transform target = FindClosestEnemy();

            if (target == null || target.gameObject == null || !IsVisible(target))
            {
                shootTimer = 0f; // Keine Gegner kein Schuss
                return;
            }

            Vector2 direction = (Vector2)(target.position - transform.position);
            Shoot(direction);

            shootTimer = 0f;
        }
    }

    Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closest = enemy.transform;
            }
        }

        return closest;
    }

    bool IsVisible(Transform target)
    {
        if (target == null) return false;
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(target.position);
        return  viewportPos.x >= 0 && viewportPos.x <= 1 &&
                viewportPos.y >= 0 && viewportPos.y <= 1 &&
                viewportPos.z > 0;
    }

    void Shoot(Vector2 direction)
    {
        if (projectilePrefab == null) return;

        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectileScript = proj.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SetDirection(direction);
        }
    }
}
