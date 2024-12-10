using Unity.VisualScripting;
using UnityEngine;

public class Floater : EnemyBase
{

    [SerializeField] float speed = 5;
    [SerializeField] float attackRange = 2f;
    [SerializeField] int damagePower;
    [SerializeField] int damageDelay;
    int timer = 0;

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        HeroKnight Knight = FindFirstObjectByType<HeroKnight>();
        Rigidbody2D tmpRigidbody2D = gameObject.GetComponentInParent<Rigidbody2D>();
        float tmpDist = Vector3.Distance(Knight.transform.position, transform.position);
        tmpRigidbody2D.AddForce((Knight.transform.position - transform.position) / tmpDist * speed);
        if(tmpDist < attackRange && timer <= 0)
        {
            timer = damageDelay;
            // Damage the enemy
            Knight.GetComponent<Health>()?.TakeDamage(damagePower);
        }
        timer--;
    }
}
