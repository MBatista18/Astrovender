using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bomb : MonoBehaviour
{
    //Declaring variables
    [Header("Explosion")]
    public float fuseTime = 5f;
    public int damage = 10;

    private void Update()
    {
        fuseTime -= Time.deltaTime;
        if (fuseTime <= 0)
        {
            Explode();
        }
    }

    //Handles the explosion logic for the bomb
    private void Explode()
    {
        Instantiate(AssetCall.instance.explosion, transform.position, Quaternion.identity).GetComponent<Explosion>().SetDamageValue(damage);

        //Destroy bomb
        Destroy(gameObject);
    }

}
