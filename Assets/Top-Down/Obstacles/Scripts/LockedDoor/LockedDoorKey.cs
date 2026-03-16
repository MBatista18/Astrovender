using UnityEngine;

public class LockedDoorKey : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            AssetCall.instance.playerSM.CollectKey();
            Destroy(gameObject);
        }
    }
}
