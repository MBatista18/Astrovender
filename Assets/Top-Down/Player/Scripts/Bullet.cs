using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Declaring variables
    [Header("Movement")]
    public float speed = 15f;
    private Vector2 bulletDirection = Vector2.up;

    [Header("LifeTime")]
    public float maxDistance = 20f;     //despawn after traveling this far
    public float maxLifetime = 3f;      //backup despawn timer

    [Header("Damage")]
    public int damage = 10;

    [Header("BulletPrep")]
    private Vector3 startPos;
    private float spawnTime;
    private Rigidbody2D bulletRigidBody;


    //Called when script is activated
    private void OnEnable()
        {
            startPos = transform.position;
            spawnTime = Time.time;
        }

    //Initialization
    private void Awake()
    {
        bulletRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Move forward (uses the bullet's own forward direction)
        transform.position += (Vector3)bulletDirection * speed * Time.deltaTime;

        //Despawn after distance
        if(Vector3.Distance(startPos, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }

        //Backup despawn after time
        if(Time.time - spawnTime >= maxLifetime)
        {
            Destroy(gameObject);
        }

    }

    //Called when another colliders enters the bullet's collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Bullet hit: {other.name} tag={other.tag}");

        //Damages only enemies
        if (other.CompareTag("Enemy"))
        {
            //Sends a message to the enemy script telling the enemy to take damage
            other.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            Debug.Log("Enemy takes damage");

            //Destroy bullet upon hit
            Destroy(gameObject);
        }

        
    }

    //Sets the direction of the bullet (Haven't been able to test this yet)
    public void SetDirection(PlayerStateMachine.facingDirection facingDirection)
    {
        Vector2 direction = Vector2.zero;

        switch (facingDirection)
        {
            case PlayerStateMachine.facingDirection.up:
                direction = Vector2.up;
                break;
            case PlayerStateMachine.facingDirection.down:
                direction = Vector2.down;
                break;
            case PlayerStateMachine.facingDirection.left:
                direction = Vector2.left;
                break;
            case PlayerStateMachine.facingDirection.right:
                direction = Vector2.right;
                break;
        }

        if(direction.sqrMagnitude > 0.0001f)
        {
            bulletDirection = direction.normalized;
        }
    }

}
