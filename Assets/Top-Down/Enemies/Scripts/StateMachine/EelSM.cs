using UnityEngine;

public class EelSM : EnemySM
{
    public EelStatePatrol statePatrol;

    public override StateBase InitialState()
    {
        return statePatrol;
    }

    public EelStateAttack stateAttack;

    public override StateBase AttackState()
    {
        return stateAttack;
    }

    public override void InstantiateStates()
    {
        base.InstantiateStates();

        statePatrol = new EelStatePatrol(this);
        stateAttack = new EelStateAttack(this);
    }

    [Header("Movement")]
    public Rigidbody2D rb;
    public float moveSpeed = 2f;
    public float patrolChangeTime = 2f;

    [Header("Chase")]
    public float chaseSpeed = 3f;
    public float shockRange = 1.5f;

    [Header("Attack")]
    public GameObject shockZonePrefab;
    public float tileSize = 1f;
    public float windupDuration = 0.5f;
    public float shockDuration = 0.4f;
    public float attackCooldown = 1.5f;
    public float postShockWaitDuration = 2f;

    [Header("Patrol Values")]
    private Bounds patrolBounds;
    [SerializeField] Vector2 patrolSize;
    [SerializeField] Vector2 patrolCenterOffset;
    [SerializeField] private float boundsBuffer = 0.1f;

    [HideInInspector] public Vector3 originalScale;
    [SerializeField] private Collider2D eelBodyCollider;

    private float directionLockTimer = 0f;
    [SerializeField] private float directionLockDuration = 0.5f;

    private Vector2 moveDirection;

    public override void InstantiateComponents()
    {
        base.InstantiateComponents();

        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        patrolBounds = new Bounds(transform.position + (Vector3)patrolCenterOffset, patrolSize);
    }

    //Functions
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 center = Application.isPlaying
            ? patrolBounds.center
            : transform.position + (Vector3)patrolCenterOffset;

        Gizmos.DrawWireCube(center, patrolSize);
    }

    public void SetMoveDirection(Vector2 dir)
    {
        moveDirection = dir.normalized;
    }

    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    public Vector2 GetRandomDiagonalDirection()
    {
        Vector2[] directions =
        {
        new Vector2(1, 1),
        new Vector2(-1, 1),
        new Vector2(1, -1),
        new Vector2(-1, -1)
    };

        return directions[Random.Range(0, directions.Length)].normalized;
    }

    public Vector2 GetNewRandomDiagonalDirection(Vector2 currentDir)
    {
        Vector2 newDir;

        do
        {
            newDir = GetRandomDiagonalDirection();
        }
        while (Vector2.Dot(newDir, currentDir) > 0.9f);

        return newDir;
    }

    //Functions determining directional movement lock
    public void UpdateDirectionLockTimer()
    {
        if (directionLockTimer > 0f)
        {
            directionLockTimer -= Time.deltaTime;
        }
    }

    public bool CanChangeDirection()
    {
        return directionLockTimer <= 0f;
    }

    public void LockDirection()
    {
        directionLockTimer = directionLockDuration;
    }

    //Functions determining patrol boundaries
    public bool IsInsideBounds()
    {
        return patrolBounds.Contains(transform.position);
    }

    public bool IsNearBoundsEdge()
    {
        Bounds patrol = patrolBounds;
        Bounds eel = eelBodyCollider.bounds;

        bool nearEdge =
            eel.min.x <= patrol.min.x + boundsBuffer ||
            eel.max.x >= patrol.max.x - boundsBuffer ||
            eel.min.y <= patrol.min.y + boundsBuffer ||
            eel.max.y >= patrol.max.y - boundsBuffer;

        //Debug.Log("Near bounds edge: " + nearEdge + " | Collider size: " + eel.size);

        return nearEdge;
    }

    public Vector2 GetBoundsCorrectedDirection()
    {
        Bounds patrol = patrolBounds;
        Bounds eel = eelBodyCollider.bounds;

        Vector2 newDir = moveDirection;

        // Detect left/right edge and flip horizontal direction
        if (eel.min.x <= patrol.min.x + boundsBuffer)
        {
            newDir.x = 1f;
        }
        else if (eel.max.x >= patrol.max.x - boundsBuffer)
        {
            newDir.x = -1f;
        }

        // Detect bottom/top edge and flip vertical direction
        if (eel.min.y <= patrol.min.y + boundsBuffer)
        {
            newDir.y = 1f;
        }
        else if (eel.max.y >= patrol.max.y - boundsBuffer)
        {
            newDir.y = -1f;
        }

        return newDir.normalized;
    }
}