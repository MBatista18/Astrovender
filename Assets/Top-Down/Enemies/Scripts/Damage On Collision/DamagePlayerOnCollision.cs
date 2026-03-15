using UnityEngine;

public class DamagePlayerOnCollision : MonoBehaviour
{
    [SerializeField] [Tooltip("Damage value applied to player, please keep this value negative")] int damageVal;

    [SerializeField] private float attackCooldown = 1.5f;
    private float nextAttackTime = 0f;

    [Header("Knockback")]
    [SerializeField] bool knocksPlayer;
    [SerializeField] float knockDuration = .5f;

    public virtual void OnCollision() { return; }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("True");

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(Time.time >= nextAttackTime)
            {
                PlayerHealth.ModifyOxygenLevel(damageVal, false, transform.position);

                if (knocksPlayer) { AssetCall.instance.playerSM.Knockback(transform.position, knockDuration); }

                OnCollision();

                // Set next allowed attack time
                nextAttackTime = Time.time + attackCooldown;
                Debug.Log("Attack is in cooldown");

            }
           
        }
    }


}
