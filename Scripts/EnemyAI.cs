using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f;
    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 movement;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null)
            return;

        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        movement = direction;

        UpdateAnimation(direction);
    }

    private void FixedUpdate()
    {
        MoveCharacter(movement);
    }

    void MoveCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
    }

    void UpdateAnimation(Vector3 direction)
    {
        if (animator == null)
            return;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                animator.SetBool("IsRight", true);
                animator.SetBool("IsLeft", false);
                animator.SetBool("IsUp", false);
                animator.SetBool("IsDown", false);
            }
            else
            {
                animator.SetBool("IsRight", false);
                animator.SetBool("IsLeft", true);
                animator.SetBool("IsUp", false);
                animator.SetBool("IsDown", false);
            }
        }
        else
        {
            if (direction.y > 0)
            {
                animator.SetBool("IsRight", false);
                animator.SetBool("IsLeft", false);
                animator.SetBool("IsUp", true);
                animator.SetBool("IsDown", false);
            }
            else
            {
                animator.SetBool("IsRight", false);
                animator.SetBool("IsLeft", false);
                animator.SetBool("IsUp", false);
                animator.SetBool("IsDown", true);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10); // Schadenanpassung
            }
            Destroy(gameObject); // Maggot wird zerstört, wenn er den Spieler berührt, aber damage kommt noch
        }
    }
}
