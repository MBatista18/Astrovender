using UnityEngine;

public class DamagePlayerOnCollision : MonoBehaviour
{
    [SerializeField] [Tooltip("Damage value applied to player, please keep this value negative")] int damageVal;

    [SerializeField] private float attackCooldown = 1.5f;
    private float nextAttackTime = 0f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("True");

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(Time.time >= nextAttackTime)
            {
                PlayerHealth.ModifyOxygenLevel(damageVal, false);

                // Set next allowed attack time
                nextAttackTime = Time.time + attackCooldown;
                Debug.Log("Attack is in cooldown");

            }
           
        }
    }
}
