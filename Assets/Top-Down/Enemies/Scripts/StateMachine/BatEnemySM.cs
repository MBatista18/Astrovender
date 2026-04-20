using UnityEngine;

public class BatEnemySM : EnemySM
{
    BatEnemyStateAttack stateAttack;

    public override StateBase AttackState()
    {
        return stateAttack;
    }

    BatEnemyStatePatrol statePatrol;
    public override StateBase InitialState()
    {
        return statePatrol;
    }

    public override void InstantiateStates()
    {
        base.InstantiateStates();

        stateAttack = new BatEnemyStateAttack(this);
        statePatrol = new BatEnemyStatePatrol(this);
    }

    private float moveSpeed = 2f;
    private Rigidbody2D rb;

    private float directionLockTimer = 0f;
    [SerializeField] private float directionLockDuration = 0.5f;

    public Transform player;

    private Vector2 moveDirection;
    private Collider2D batCollider;

    [Header("Patrol Values")]
    //[SerializeField] private Collider2D patrolBounds;
    private Bounds patrolBounds;
    [SerializeField] Vector2 patrolSize;
    [SerializeField] Vector2 patrolCenterOffset;
    [SerializeField] private float boundsBuffer = 0.5f;

    private Vector2 startPosition;

    private bool hitWall;
    public override void OnShieldReaction()
    {
        Debug.Log("Shield reaction bat");
        GetStateKnockback().SetKnockback((Vector2) (transform.position - AssetCall.instance.playerSM.transform.position), 1f);
        ChangeState(GetStateKnockback());
    }

    public override void InstantiateComponents()
    {
        base.InstantiateComponents();

        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        batCollider = GetComponent<Collider2D>();
        patrolBounds = new Bounds(transform.position + (Vector3) patrolCenterOffset, patrolSize);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + (Vector3)patrolCenterOffset, patrolSize);
    }

    public void SetMoveDirection(Vector2 dir)
    {
        moveDirection = dir.normalized;
    }

    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    //Many functions to try and keep the enemy from hitting the wall or turning away from it if it does
    public bool DidHitWall()
    {
        return hitWall;
    }

    public void ClearHitWall()
    {
        hitWall = false;
    }

    public void ReflectDirection(Vector2 collisionNormal)
    {
        Vector2 reflected = Vector2.Reflect(moveDirection, collisionNormal);

        float x = reflected.x >= 0 ? 1f : -1f;
        float y = reflected.y >= 0 ? 1f : -1f;

        moveDirection = new Vector2(x, y).normalized;
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            Debug.Log("HIT WALL");
            hitWall = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            if (collision.contacts.Length > 0)
            {
                Vector2 normal = collision.contacts[0].normal;
                SetMoveDirection(GetWallAdjustedDiagonalDirection(normal));
            }
        }
    }*/

    public Vector2 GetDiagonalOnlyDirection(Vector2 targetPosition)
    {
        Vector2 rawDirection = (targetPosition - (Vector2)transform.position).normalized;

        float x = rawDirection.x >= 0 ? 1f : -1f;
        float y = rawDirection.y >= 0 ? 1f : -1f;

        return new Vector2(x, y).normalized;
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

    public Vector2 GetWallAdjustedDiagonalDirection(Vector2 collisionNormal)
    {
        Vector2 newDir = moveDirection;

        if (Mathf.Abs(collisionNormal.x) > 0.5f)
        {
            newDir.x *= -1f;
        }

        if (Mathf.Abs(collisionNormal.y) > 0.5f)
        {
            newDir.y *= -1f;
        }

        return newDir.normalized;
    }

    public Vector2 GetForcedTurnDirection()
    {
        Vector2 newDir = moveDirection;

        // Flip one axis randomly
        if (Random.value < 0.5f)
        {
            newDir.x *= -1f;
        }
        else
        {
            newDir.y *= -1f;
        }

        return newDir.normalized;
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

    public Vector2 GetBoundsCorrectedDirection()
    {
        Bounds patrol = patrolBounds;
        Bounds bat = batCollider.bounds;

        Vector2 newDir = moveDirection;

        if (bat.min.x <= patrol.min.x + boundsBuffer)
        {
            newDir.x = 1f;
        }
        else if (bat.max.x >= patrol.max.x - boundsBuffer)
        {
            newDir.x = -1f;
        }

        if (bat.min.y <= patrol.min.y + boundsBuffer)
        {
            newDir.y = 1f;
        }
        else if (bat.max.y >= patrol.max.y - boundsBuffer)
        {
            newDir.y = -1f;
        }

        return newDir.normalized;
    }

    public bool IsNearBoundsEdge()
    {
        Bounds patrol = patrolBounds;
        Bounds bat = batCollider.bounds;

        return bat.min.x <= patrol.min.x + boundsBuffer ||
               bat.max.x >= patrol.max.x - boundsBuffer ||
               bat.min.y <= patrol.min.y + boundsBuffer ||
               bat.max.y >= patrol.max.y - boundsBuffer;
    }


    public override void UpdateFunctions()
    {
        base.UpdateFunctions();

        if (GetCurrentState() == GetStateKnockback()) { return; }

        if (moveDirection.y < 0)
        {
            if (moveDirection.x < 0)
            {
                GetAnimator().Play("EyeballForward");
            }
            else
            {
                GetAnimator().Play("EyeballLeft");
            }
        }
        else
        {
            if (moveDirection.x < 0)
            {
                GetAnimator().Play("EyeballLeft");
            }
            else
            {
                GetAnimator().Play("EyeballBackward");
            }
        }
    }
}
