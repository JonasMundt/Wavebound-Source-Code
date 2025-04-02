using System.Collections;
using UnityEngine;

public class AuraDamage : MonoBehaviour
{
    public float damage = 0.5f;
    public float tickInterval = 0.5f;
    public float radius = 2f;

    [SerializeField] private Transform damageOrigin; //Spieleraura

    private void OnEnable()
    {
        StartCoroutine(DamageRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator DamageRoutine()
    {
        while (true)
        {
            DealDamage();
            yield return new WaitForSeconds(tickInterval);
        }
    }

    private void DealDamage()
    {
        Vector2 origin = damageOrigin != null ? (Vector2)damageOrigin.position : (Vector2)transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, radius);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    // Debug.Log($"Aura hits {hit.name} for {damage} damage.");
                }
            }
        }
    }

    // Optional wegen Debuggen
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 origin = damageOrigin != null ? (Vector2)damageOrigin.position : (Vector2)transform.position;
        Gizmos.DrawWireSphere(origin, radius);
    }
}
