using UnityEngine;

public class CollectWeapon : MonoBehaviour
{
    [SerializeField] bool gun;
    [SerializeField] bool bomb;
    [SerializeField] bool shield;

    private void Start()
    {
        if ((gun && GameManager.Instance.currentdataObj.hasGun) ||
            (shield && GameManager.Instance.currentdataObj.hasShield) ||
            (bomb && GameManager.Instance.currentdataObj.hasBombs)) { Destroy(gameObject); }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return;
        }

        AssetCall.instance.playerSM.GetAudioCall().CallAudioClip("CollectAbility");

        if (gun) { GameManager.Instance.currentdataObj.hasGun = true; }
        if (bomb) { GameManager.Instance.currentdataObj.hasBombs = true; }
        if (shield) { GameManager.Instance.currentdataObj.hasShield = true; }

        Destroy(gameObject);
    }
}
