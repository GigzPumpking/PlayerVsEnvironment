using UnityEngine;

public class Spike : EnemyBase
{
    [SerializeField] private int damagePower = 10; // Damage dealt to the player on touch

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object is the player
        if (collision.collider.CompareTag("Player"))
        {
            // Attempt to deal damage to the player's Health component
            Health playerHealth = collision.collider.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damagePower);
                Debug.Log("Player took damage from spike: " + damagePower);
            }
        }
    }
}
