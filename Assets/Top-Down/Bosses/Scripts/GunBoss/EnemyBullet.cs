using UnityEngine;

public class EnemyBullet : Bullet
{
    //Called when another colliders enters the bullet's collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger");

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerHealth.ModifyOxygenLevel(damage, false);
            Destroy(gameObject);
        }
    }
}
