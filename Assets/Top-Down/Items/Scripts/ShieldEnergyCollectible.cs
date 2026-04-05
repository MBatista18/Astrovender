using UnityEngine;

public class ShieldEnergyCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }
        if (GameManager.Instance == null) { return; }

        PlayerManager.ModifyShieldHealth(Random.Range(5, 8));

        AssetCall.instance.playerSM.GetAudioCall().CallAudioClip("CollectResource");

        Destroy(gameObject);
    }
}
