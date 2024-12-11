using UnityEngine;

public class Floater : EnemyBase
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float patrolRange = 5f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int damagePower = 10;
    [SerializeField] private float damageCooldown = 1f;

    private Rigidbody2D rb;
    private bool isMovingRight = true;
    private Vector2 initialPosition;
    private float damageTimer = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
    }

    private new void Update()
    {
        base.Update();
        HandleMovement();
        HandleDamageTimer();
        CheckForAttack();
    }

    private void HandleMovement()
    {
        float offset = isMovingRight ? patrolRange : -patrolRange;
        float targetX = initialPosition.x + offset;

        // Change direction if the Floater reaches the patrol limit
        if ((isMovingRight && transform.position.x >= targetX) || (!isMovingRight && transform.position.x <= targetX))
        {
            isMovingRight = !isMovingRight;
        }

        // Apply horizontal movement
        if (rb != null)
        {
            float direction = isMovingRight ? 1f : -1f;
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        }
    }

    private void HandleDamageTimer()
    {
        if (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
        }
    }

    private void CheckForAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Debug.Log("Hit Player: " + hit.name);

                if (damageTimer <= 0)
                {
                    damageTimer = damageCooldown;
                    Health playerHealth = hit.GetComponent<Health>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(damagePower);
                    }
                }
                break;
            }
        }

        // Visualize the attack range in the editor
        Debug.DrawLine(transform.position - Vector3.right * attackRange, transform.position + Vector3.right * attackRange, Color.red);
    }
}
