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

    private Vector3 startPos;
    private float spawnTime;
    private Rigidbody2D bulletRigidBody;



    private void OnEnable()
        {
            startPos = transform.position;
            spawnTime = Time.time;
        }

    private void Awake()
    {
        bulletRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Move forward (uses the bullet's own forward direction)
        transform.position += transform.up * speed * Time.deltaTime;

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

    public void SetDirection(Vector2 direction)
    {
        if(direction.sqrMagnitude > 0.0001f)
        {
            bulletDirection = direction.normalized;
        }
    }

}
