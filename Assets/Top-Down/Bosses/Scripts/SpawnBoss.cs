using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnBoss : MonoBehaviour
{
    // this class is to prevent the boss from running in the background

    [SerializeField] GameObject boss;

    private void Awake()
    {
        boss.SetActive(false);
    }

    private void Start()
    {
        //if (GameManager.Instance.defeatedBombsBoss)

        if (GameManager.Instance.currentdataObj.dungeons[SceneManager.GetActiveScene().name].defeatedBoss)
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
