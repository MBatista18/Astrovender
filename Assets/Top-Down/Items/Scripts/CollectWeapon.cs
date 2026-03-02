using UnityEngine;

public class CollectWeapon : MonoBehaviour
{
    [SerializeField] bool gun;
    [SerializeField] bool bomb;
    [SerializeField] bool shield;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return;
        }

        if (gun) { GameManager.Instance.collectedGun = true; }
        if (bomb) { GameManager.Instance.collectedBombs = true; }
        if (shield) { GameManager.Instance.collectedShield = true; }

        Destroy(gameObject);
    }
}
