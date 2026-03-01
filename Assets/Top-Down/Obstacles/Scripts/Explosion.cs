using UnityEngine;
using System.Collections.Generic;

public class Explosion : MonoBehaviour
{
    [SerializeField] int damage;
    public void SetDamageValue(int _damage) { damage = _damage; }

    private readonly HashSet<Collider2D> exploded = new HashSet<Collider2D>(); // make sure we're not exploding the same object more than once

    private void Start()
    {
        GetComponent<Animator>().Play("FireExplosionClip");

        // aligns the explosion to the grid.

        Vector2 refPosition = transform.position;
        Vector2 newPosition = new Vector2(Mathf.Round(refPosition.x), Mathf.Round(refPosition.y));

        transform.position = new Vector2(transform.position.x < newPosition.x ? newPosition.x - .5f : newPosition.x + .5f,
            transform.position.y < newPosition.y ? newPosition.y - .5f : newPosition.y + .5f);
    }

    float checkTimer = 0.1f;
    float currentCheckTimer = 0;

    float explosionDuration = 0.7f;

    private void Update()
    {
        Explode();

        explosionDuration -= Time.deltaTime;
        if (explosionDuration <= 0) { Destroy(gameObject); }
    }

    void Explode()
    {
        if (currentCheckTimer > 0) { currentCheckTimer -= Time.deltaTime; return; } 
            // puts the explosion check against a timer delay so it's not checking every frame, but every few frames
        else { currentCheckTimer = checkTimer; }

        RaycastHit2D[] raycastHits = Physics2D.BoxCastAll(transform.position, new Vector2(3, 3), 0f, Vector2.zero, 0f);

        foreach (RaycastHit2D ray in raycastHits)
        {
            if (exploded.Contains(ray.collider)) { continue; }
            else { exploded.Add(ray.collider); }

            string layerName = LayerMask.LayerToName(ray.collider.gameObject.layer);

            switch (layerName)
            {
                case "Enemy":
                    ray.collider.gameObject.GetComponent<EnemySM>().TakeDamage(damage);
                    break;
                case "Player":
                    PlayerHealth.ModifyOxygenLevel(-damage, false);

                    // knocks the player either horizontally or vertically (horizontally if they're farther on the x axis, verticaly if they're farther on the y-axis)

                    Vector2 distanceDiff = new Vector2(AssetCall.instance.playerSM.transform.position.x - transform.position.x,
                        AssetCall.instance.playerSM.transform.position.y - transform.position.y).normalized;


                    AssetCall.instance.playerSM.Knockback(
                        Mathf.Abs(distanceDiff.x) > Mathf.Abs(distanceDiff.y) ? Vector2.right * Mathf.Sign(distanceDiff.x) : Vector2.up * Mathf.Sign(distanceDiff.y),
                        1.5f);
                    break;
                case "Destructible":
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
