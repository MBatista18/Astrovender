using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bomb : MonoBehaviour
{
    //Declaring variables
    [Header("Explosion")]
    public float fuseTime = 5f;
    public Vector2 boxSize = new Vector2(2f, 2f); //Square-ish area
    public int damage = 25;

    [Header("Targets")] //Set to enemy or destructible layers and tags for the bomb to affect those objects
    public LayerMask enemyLayer;
    public LayerMask destructibleLayer;

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

    //Handles the explosion logic for the bomb
    private void Explode()
    {
        Debug.Log("Explosion went off");
        
        //Find everything in a square area
        Collider2D[] enemyHits = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f,  enemyLayer);
        Collider2D[] destructibleHits = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f, destructibleLayer);

        //Apply damage / destruction
        foreach (Collider2D other in enemyHits)
        {
            Debug.Log("Reading enemy hit list");
            //Enemy explosion damage
            if (other.CompareTag("Enemy"))
            {
                other.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
                Debug.Log("Enemy takes explosion damage");
            }
        }

        foreach (Collider2D other in destructibleHits)
        {
            Debug.Log("Reading destructible hit list");
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

    //Coroutine for how long bomb needs to wait before it explodes
    IEnumerator FuseTimer()
    {
        yield return new WaitForSeconds(fuseTime);
        Explode();
    }

    //Debug for explosion radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, boxSize);
    }

}
