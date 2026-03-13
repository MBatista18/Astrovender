using UnityEngine;

public class EnemyBullet : Bullet
{
    //Called when another colliders enters the bullet's collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Trigger");

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerHealth.ModifyOxygenLevel(damage, false, transform.position);
            Destroy(gameObject);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            Destroy(gameObject);
        }
    }
}
