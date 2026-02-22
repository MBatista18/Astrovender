using UnityEngine;

public class DamagePlayerOnCollision : MonoBehaviour
{
    [SerializeField] [Tooltip("Damage value applied to player, please keep this value negative")] int damageVal;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("True");

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerHealth.ModifyOxygenLevel(damageVal, false);
        }
    }
}
