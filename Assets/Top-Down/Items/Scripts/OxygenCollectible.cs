using UnityEngine;

public class OxygenCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }
        if (GameManager.Instance == null) { return; }

        PlayerHealth.ModifyOxygenLevel(Random.Range(20, 30), true);

        Destroy(gameObject);
    }
}
