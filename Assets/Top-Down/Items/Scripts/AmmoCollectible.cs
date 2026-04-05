using UnityEngine;

public class AmmoCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }
        if (GameManager.Instance == null) { return; }

        PlayerManager.ModifyAmmoCount(Random.Range(4, 8));

        AssetCall.instance.playerSM.GetAudioCall().CallAudioClip("CollectResource");

        Destroy(gameObject);
    }
}
