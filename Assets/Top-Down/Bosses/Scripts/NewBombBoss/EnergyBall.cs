using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    [Header("Arc Movement")]
    [SerializeField] float arcHeight = 3f;
    [SerializeField] float travelTime = 1.5f;

    Vector2 startPos;
    Vector2 targetPosition;
    float travelStartTime;
    bool launched;

    [Header("LifeTime")]
    [SerializeField] float maxLifetime = 3f;      // backup despawn timer

    float spawnTime;

    [Header("Damage")]
    [SerializeField] int damage = 10;

    private void OnEnable()
    {
        spawnTime = Time.time;
    }

    void Update()
    {
        if (launched)
        {
            // Interpolate on XY plane and add an arc offset in screen Y for visual arc.
            float elapsed = Time.time - travelStartTime;
            float t = travelTime > 0f ? Mathf.Clamp01(elapsed / travelTime) : 1f;

            // Base linear interpolation between start and target
            Vector2 basePos = Vector2.Lerp(startPos, targetPosition, t);

            // Add vertical arc using a sin curve (peaks at t=0.5)
            float height = Mathf.Sin(Mathf.PI * t) * arcHeight;

            // We represent the arc by offsetting the sprite along world Y
            Vector3 worldPos = new Vector3(basePos.x, basePos.y + height, transform.position.z);
            transform.position = worldPos;

            // Arrived at target
            if (t >= 1f)
            {
                Destroy(gameObject);
                return;
            }
        }

        // Backup despawn after time
        if (Time.time - spawnTime >= maxLifetime)
        {
            Destroy(gameObject);
        }
    }

    // Start Movement
    public void Launch(Vector2 targetPosition)
    {
        this.startPos = new Vector2(transform.position.x, transform.position.y);
        this.targetPosition = targetPosition;
        travelStartTime = Time.time;
        spawnTime = Time.time;
        launched = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //PlayerHealth.ModifyOxygenLevel(damage, false, transform.position);
            Destroy(gameObject);
            return;
        }
    }
}
