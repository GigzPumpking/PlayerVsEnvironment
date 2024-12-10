using Unity.VisualScripting;
using UnityEngine;

public class SimpleEnemy : EnemyBase
{
    [SerializeField] bool bMovingRight = true;
    [SerializeField] float speed = 10;

    [SerializeField] float attackRange = 2f;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] int damagePower;
    [SerializeField] int damageDelay;

    int timer = 0;

    [SerializeField] private Sensor leftFloorSensor;
    [SerializeField] private Sensor rightFloorSensor;
    [SerializeField] private Sensor leftSensor;
    [SerializeField] private Sensor rightSensor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (rightSensor.State() || !rightFloorSensor.State())
        {
            bMovingRight = false;
        }
        if (leftSensor.State() || !leftFloorSensor.State())
        {
            bMovingRight = true;
        }
        float m_facingDirection = 1f;
        if (!bMovingRight)
        {
            m_facingDirection = -1f;
        }

        Vector2 rayDirection = new Vector2(m_facingDirection, 0f);

        // Cast a ray to detect enemies
        RaycastHit2D objectHit = Physics2D.Raycast(transform.position, rayDirection, attackRange, enemyLayer);
        // Visualize the ray
        // Debug.DrawRay(transform.position + new Vector3(0f, 0.5f, 0f), rayDirection * attackRange, Color.green, 3f);

        bool bInRange = false;
        // If we hit something
        if (objectHit.collider != null)
        {
            if (objectHit.collider.CompareTag("Player") && timer <= 0)
            {
                timer = damageDelay;
                // Damage the enemy
                objectHit.collider.GetComponent<Health>()?.TakeDamage(damagePower);
            }
            bInRange = true;
        }
        timer--;
        if (!bInRange)
        {
            if (bMovingRight)
            {
                Rigidbody2D tmpRigidbody2D = gameObject.GetComponentInParent<Rigidbody2D>();
                tmpRigidbody2D.linearVelocityX = speed;
            }
            else
            {
                Rigidbody2D tmpRigidbody2D = gameObject.GetComponentInParent<Rigidbody2D>();
                tmpRigidbody2D.linearVelocityX = -speed;
            }
        }
        else
        {
            Rigidbody2D tmpRigidbody2D = gameObject.GetComponentInParent<Rigidbody2D>();
            tmpRigidbody2D.linearVelocityX = 0f;
        }
    }
}
