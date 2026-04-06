using UnityEngine;

public class TrackingProjectile : MonoBehaviour
{

    [Header("Damage")]
    [SerializeField] int damage = 10;
    [SerializeField] float turnSpeed = 10f;
    [SerializeField] float turnMultiplier = 5f;
    [SerializeField] float speed = 2f;

    private void Update()
    {
        /*Vector3 direction = AssetCall.instance.playerSM.transform.position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, turnSpeed * Mathf.Deg2Rad * Time.deltaTime, 1f);
        newDirection = new Vector3(0, 0, newDirection.z); transform.rotation = Quaternion.LookRotation(newDirection);*/


        Quaternion facing = Quaternion.AngleAxis((Mathf.Atan2(AssetCall.instance.playerSM.transform.position.y - transform.position.y, AssetCall.instance.playerSM.transform.position.x - transform.position.x)
                * Mathf.Rad2Deg), Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, facing, turnSpeed * turnMultiplier * Time.deltaTime);

        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerHealth.ModifyOxygenLevel(damage, false, transform.position);
            Destroy(gameObject);
            return;
        }
    }
}
