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

        if (gun) { GameManager.Instance.currentdataObj.hasGun = true; PlayerManager.ammoCount = PlayerManager.GetMaxAmmoCount(); }
        if (bomb) { GameManager.Instance.currentdataObj.hasBombs = true; PlayerManager.bombCount = PlayerManager.GetMaxBombCount(); }
        if (shield) { GameManager.Instance.currentdataObj.hasShield = true; PlayerManager.SetCurrentShieldHealth(PlayerManager.GetMaxShieldHealth()); }

        Destroy(gameObject);
    }
}
