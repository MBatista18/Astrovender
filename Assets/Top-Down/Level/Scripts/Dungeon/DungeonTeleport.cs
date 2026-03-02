using UnityEngine;

public class DungeonTeleport : MonoBehaviour
{
    [SerializeField] Transform newPosition;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            AssetCall.instance.playerSM.transform.position = newPosition.position;
        }
    }
}
