using UnityEngine;

public class BuddyAI : MonoBehaviour
{
    public Transform player;
    public GameObject projectilePrefab;
    public float followSpeed = 5f;
    public float shootInterval = 1f;
    private float shootTimer;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Kamera folgt Spieelr
        if (player != null)
        {
            Vector3 targetPosition = player.position + new Vector3(1.5f, 1f, 0);
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }

        // Schießen
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            GameObject enemy = FindClosestVisibleEnemy();
            if (enemy != null)
            {
                Vector2 direction = (enemy.transform.position - transform.position).normalized;
                GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                proj.GetComponent<Rigidbody2D>().velocity = direction * 15f;
            }
            shootTimer = 0f;
        }
    }

    GameObject FindClosestVisibleEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDist = Mathf.Infinity;
        GameObject closest = null;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            Vector3 screenPoint = mainCamera.WorldToViewportPoint(enemy.transform.position);
            bool isVisible = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

            if (isVisible)
            {
                float dist = Vector2.Distance(transform.position, enemy.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = enemy;
                }
            }
        }

        return closest;
    }
}
