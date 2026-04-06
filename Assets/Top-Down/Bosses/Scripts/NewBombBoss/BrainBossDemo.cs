using UnityEngine;

public class BrainBossDemo : MonoBehaviour
{
    [SerializeField] Bomb[] bombs;

    [SerializeField] Port port;

    private void Start()
    {
        port.ActivatePort();
    }

    private void Update()
    {
        foreach (Bomb a in bombs)
        {
            a.fuseTime = 10000;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            foreach(Bomb a in bombs)
            {

                a.fuseTime = 0;
            }
        }
    }
}
