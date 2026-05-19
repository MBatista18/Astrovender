using UnityEngine;

public class FinalDoorKey : MonoBehaviour
{
    public enum KeyType {left, right }

    public KeyType thisKeyType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            switch (thisKeyType)
            {
                case KeyType.left:
                    GameManager.Instance.currentdataObj.finalKeyLeft = true;
                    break;
                case KeyType.right:
                    GameManager.Instance.currentdataObj.finalKeyRight = true;
                    break;
            }

            AssetCall.instance.playerSM.GetAudioCall().CallAudioClip("CollectKey");

            Destroy(gameObject);
        }
    }
}
