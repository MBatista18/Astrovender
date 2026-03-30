using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sword : MonoBehaviour
{
    //Declaring variables
    [Header("Damage")]
    public int damage = 5;

    //Track who we've hit during the current swing so we don't multi-hit the same enemy
    private readonly HashSet<Collider2D> _hitThisSwing = new HashSet<Collider2D> ();

    //Is called once when the script is enabled
    public void BeginSwing()
    {
        _hitThisSwing.Clear ();
    }

    //Actiavtes when another collider enters the sword's collision
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            // other.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            other.GetComponent<EnemySM>().TakeDamage(damage, AssetCall.instance.playerSM.transform.position);
            //Debug.Log("Enemy gets hit by the sword");
        }
    }

}
