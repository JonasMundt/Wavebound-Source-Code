using UnityEngine;
using System.Collections;
public class BossAI : MonoBehaviour
{
    public float speed = 1.5f;
    private Transform player;
    private Animator animator;
    private GameOverManager gameOverManager;
    private EnemySpawner enemySpawner; 
    private bool hasAttacked = false; 
    private bool bossIsDead = false; 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        gameOverManager = FindObjectOfType<GameOverManager>();
        enemySpawner = FindObjectOfType<EnemySpawner>(); 

        if (gameOverManager == null)
        {
            Debug.LogWarning("⚠️ GameOverManager wurde im BossAI nicht gefunden."); //Debuggen
        }

        if (enemySpawner == null)
        {
            Debug.LogWarning("⚠️ EnemySpawner wurde im BossAI nicht gefunden."); //Debuggen
        }
    }

    void Update()
    {
        if (player != null && !hasAttacked && !bossIsDead)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)(direction * speed * Time.deltaTime);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (angle >= 45 && angle <= 135)
            {
                SetAnimatorBools(true, false, false, false);
            }
            else if (angle >= -135 && angle <= -45)
            {
                SetAnimatorBools(false, true, false, false);
            }
            else if (angle > -45 && angle < 45)
            {
                SetAnimatorBools(false, false, false, true);
            }
            else
            {
                SetAnimatorBools(false, false, true, false);
            }
        }
    }

    void SetAnimatorBools(bool up, bool down, bool left, bool right)
    {
        animator.SetBool("IsUp", up);
        animator.SetBool("IsDown", down);
        animator.SetBool("IsLeft", left);
        animator.SetBool("IsRight", right);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasAttacked)
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(200);
            }
            hasAttacked = true;
        }
    }

    public void Die()
{
    if (bossIsDead) return;

    Debug.Log("🔥 Mantis wurde besiegt!"); //Debuggen
    bossIsDead = true;

    gameObject.tag = "Untagged";

    if (enemySpawner != null)
    {
        enemySpawner.BossDied();
        Debug.Log("✅ EnemySpawner wurde benachrichtigt."); //Debuggen
    }

    StartCoroutine(TriggerEndScreenDelayed());
}
    
private IEnumerator TriggerEndScreenDelayed()
{
    yield return new WaitForSeconds(0.5f); // kurz warten, bis der Boss wirklich weg ist

    if (gameOverManager != null)
    {
        gameOverManager.ShowEndScreen();
        Debug.Log("✅ Endscreen wird angezeigt!"); //Debuggen
    }
    else
    {
        Debug.LogError("❌ GameOverManager nicht gefunden!"); //Debuggen
    }

    Destroy(gameObject);
}

}
