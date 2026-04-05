using UnityEngine;

public class CoinCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player")) { return; }
        if (GameManager.Instance == null) { return; }

        GameManager.Instance.IncrementCoins(Random.Range(1,4));

        AssetCall.instance.playerSM.GetAudioCall().CallAudioClip("CollectCoin");

        Destroy(gameObject);
    }
}
