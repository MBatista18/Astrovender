using UnityEngine;

public class PlantSM : EnemySM
{
    PlantAttackState stateAttack;

    public override StateBase AttackState()
    {
        return stateAttack;
    }

    PlantPatrolState statePatrol;

    public override StateBase InitialState()
    {
        return statePatrol;
    }

    public override void InstantiateStates()
    {
        base.InstantiateStates();

        stateAttack = new PlantAttackState(this);
        statePatrol = new PlantPatrolState(this);
    }

    [Header("References")]
    [SerializeField] private Transform player;
    [HideInInspector] public Rigidbody2D rb;

    [Header("Detection")]
    [SerializeField] private BoxCollider2D lungeBounds;
    [SerializeField] private BoxCollider2D detectionBounds;
    public float detectionRange = 4f;

    [Header("Attack")]
    public float peekDuration = 0.75f;
    public float lungeSpeed = 8f;
    public float recedeSpeed = 5f;
    public float maxLungeDistance = 3f;
    public float cooldownDuration = 1f;
    public float returnStopDistance = 0.05f;

    [HideInInspector] public Vector2 startPosition;
    [HideInInspector] public Vector2 lungeTarget;

    public override void InstantiateComponents()
    {
        base.InstantiateComponents();

        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    public bool PlayerInDetectionRange()
    {
        if (player == null || detectionBounds == null)
            return false;

        return detectionBounds.bounds.Contains(player.position);
    }

    public bool PlayerInLungeRange()
    {
        if (player == null || lungeBounds == null)
            return false;

        return lungeBounds.bounds.Contains(player.position);
    }

    public Vector2 GetClampedLungeTarget()
    {
        if (player == null || lungeBounds == null)
            return startPosition;

        Vector2 rawTarget = player.position;
        Bounds bounds = lungeBounds.bounds;

        float x = Mathf.Clamp(rawTarget.x, bounds.min.x, bounds.max.x);
        float y = Mathf.Clamp(rawTarget.y, bounds.min.y, bounds.max.y);

        return new Vector2(x, y);
    }
}
