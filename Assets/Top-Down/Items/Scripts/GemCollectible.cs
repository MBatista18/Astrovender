using UnityEngine;

public class GemCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }
        if (GameManager.Instance == null) { return; }

        GameManager.Instance.IncrementGems(1);

        Destroy(gameObject);
    }
}
