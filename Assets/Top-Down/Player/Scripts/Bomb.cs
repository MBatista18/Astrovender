using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    [Header("Explosion")]
    public float fuseTime = 5f;
    public Vector2 boxSize = new Vector2(2f, 2f); //Square-ish area
    public int damage = 25;

    [Header("Targets")]
    public LayerMask hitLayers; //Set to enemy and destructible layers for the bomb to affect those objects

    [Header("VFX")]
    public GameObject explosionVfxPrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(FuseTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Explode()
    {
        Debug.Log("Explosion went off");
        
        //Find everything in a square area
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f,  hitLayers);

        //Apply damage / destruction
        foreach(Collider2D other in hits)
        {
            //Enemy explosion damage
            if (other.CompareTag("Enemy"))
            {
                other.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
                Debug.Log("Enemy takes explosion damage");
            }

            //Destructible objects
            if (other.CompareTag("Destructible"))
            {
                other.SendMessage("Destroy", SendMessageOptions.DontRequireReceiver);
                Debug.Log("Object is destroyed");
            }
        }
        
        //Apply VFX
        if (explosionVfxPrefab != null)
        {
            Instantiate(explosionVfxPrefab, transform.position, Quaternion.identity);
        }

        //Destroy bomb
        Destroy(gameObject);
    }

    IEnumerator FuseTimer()
    {
        yield return new WaitForSeconds(fuseTime);
        Explode();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, boxSize);
    }

}
