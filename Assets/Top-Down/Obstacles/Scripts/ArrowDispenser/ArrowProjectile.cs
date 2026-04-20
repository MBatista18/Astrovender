using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArrowProjectile : MonoBehaviour, IShieldResponse
{
    [Header("Arrow Behavior")]
    [SerializeField] private float speed = 6f;
    [SerializeField] private float lifetime = 4f;

    [Header("Damage Settings")]
    [SerializeField][Tooltip("Damage value applied to player, please keep this value negative")] int damageVal;

    private Vector2 moveDirection = Vector2.right;

    private void Start()
    {
        //Destroys arrow after lifetime amount passes if nothing is hit
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        //Moves the arrow
        transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 direction)
    {
        //Sets arrow move direction
        moveDirection = direction.normalized;
    }

    public void OnShieldAttack()
    {
        Destroy(gameObject);
    }

    //Damages the player upon entering player collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit something");
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Hit player");
            PlayerHealth.ModifyOxygenLevel(damageVal, false, transform.position, this);
        }

        // Destroy arrow when it hits something solid
        if (collision.isTrigger)
            {
                Destroy(gameObject);
            }
    }
    
}