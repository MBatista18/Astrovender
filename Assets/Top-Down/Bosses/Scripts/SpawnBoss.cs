using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    // this class is to prevent the boss from running in the background

    [SerializeField] GameObject boss;

    [SerializeField] [Tooltip("0 = bomb boss, 1 = gun boss")] [Range(0, 1)] int bosstype;

    private void Awake()
    {
        boss.SetActive(false);
    }

    private void Start()
    {
        bool isTrue = false;

        if (bosstype == 0 && GameManager.Instance.defeatedBombsBoss)
        {
            isTrue = true;
        }
        else if (bosstype == 1 && GameManager.Instance.defeatedBombsBoss)
        {
            isTrue = true;
        }

        if (isTrue)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            boss.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
