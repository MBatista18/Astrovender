using UnityEngine;

public class BombCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }
        if (GameManager.Instance == null) { return; }

        PlayerManager.ModifyBombCount(Random.Range(3, 5));

        AssetCall.instance.playerSM.GetAudioCall().CallAudioClip("CollectResource");

        Destroy(gameObject);
    }
}
