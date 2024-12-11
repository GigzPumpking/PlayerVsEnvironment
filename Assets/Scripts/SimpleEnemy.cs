using UnityEngine;

public class SimpleEnemy : EnemyBase
{
    [SerializeField] private bool isMovingRight = true;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int damagePower = 10;
    [SerializeField] private float damageCooldown = 1f;

    [SerializeField] private Sensor leftFloorSensor;
    [SerializeField] private Sensor rightFloorSensor;
    [SerializeField] private Sensor leftWallSensor;
    [SerializeField] private Sensor rightWallSensor;

    private Rigidbody2D rb;
    private float damageTimer = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        // Check sensors to determine direction
        if (rightWallSensor.State() || !rightFloorSensor.State())
        {
            isMovingRight = false;
        }
        if (leftWallSensor.State() || !leftFloorSensor.State())
        {
            isMovingRight = true;
        }

        // Apply movement
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
        float facingDirection = isMovingRight ? 1f : -1f;
        Vector2 rayDirection = new Vector2(facingDirection, 0f);

        // Cast a ray to detect the player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, attackRange, enemyLayer);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Debug.Log("Hit Player: " + hit.collider.name);

            if (damageTimer <= 0)
            {
                damageTimer = damageCooldown;
                Health playerHealth = hit.collider.GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damagePower);
                }
            }
        }

        // Visualize the ray in the editor
        Debug.DrawRay(transform.position, rayDirection * attackRange, Color.green);
    }
}
