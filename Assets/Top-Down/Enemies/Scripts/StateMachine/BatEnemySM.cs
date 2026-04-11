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

    public Transform player;

    private Vector2 moveDirection;

    private bool hitWall;

    public override void InstantiateComponents()
    {
        base.InstantiateComponents();

        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    public void SetMoveDirection(Vector2 dir)
    {
        moveDirection = dir.normalized;
    }

    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    public bool DidHitWall()
    {
        return hitWall;
    }

    public void ClearHitWall()
    {
        Debug.Log("Not hitting wall");
        hitWall = false;
    }

    public void ReflectDirection(Vector2 collisionNormal)
    {
        Vector2 reflected = Vector2.Reflect(moveDirection, collisionNormal);

        float x = reflected.x >= 0 ? 1f : -1f;
        float y = reflected.y >= 0 ? 1f : -1f;

        moveDirection = new Vector2(x, y).normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
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
    }

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

}
