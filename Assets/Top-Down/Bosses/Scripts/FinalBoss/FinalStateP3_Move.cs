using UnityEngine;

public class FinalStateP3_Move : StateBase
{
    FinalBossSM sm;

    public FinalStateP3_Move(StateMachineBase _sm) : base(_sm)
    {
        sm = (FinalBossSM)_sm;
    }

    float timerOverall;
    float directionTimer;

    float shootTimer;
    float MAXshootTimer = 1f;

    public override void thisStart()
    {
        base.thisStart();

        sm.GetAudioCall().CallAudioClip("FlyAway");

        directionTimer = 0;
        timerOverall = Random.Range(4f, 6f);
        shootTimer = MAXshootTimer;
    }

    Vector2 movementDirection;

    public override void thisUpdate()
    {
        base.thisUpdate();

        directionTimer -= Time.deltaTime;
        timerOverall -= Time.deltaTime;
        shootTimer -= Time.deltaTime;

        if (timerOverall <= 0)
        {
            sm.ChangeState(sm.stateP3_Fire);
            return;
        }

        if (directionTimer <= 0)
        {
            directionTimer = Random.Range(0.7f, 2f);

            RecalculateDirection();
            PlayAnim();
        }

        if (shootTimer <= 0)
        {
            shootTimer = MAXshootTimer;
            Shoot();
        }
    }

    void RecalculateDirection()
    {
        float minX = Physics2D.Raycast(sm.transform.position, Vector2.left, 2f, LayerMask.GetMask("Walls", "Destructible")) ? 0 : -1;
        float maxX = Physics2D.Raycast(sm.transform.position, Vector2.right, 2f, LayerMask.GetMask("Walls", "Destructible")) ? 0 : 1;
        float minY = Physics2D.Raycast(sm.transform.position, Vector2.down, 2f, LayerMask.GetMask("Walls", "Destructible")) ? 0 : -1;
        float maxY = Physics2D.Raycast(sm.transform.position, Vector2.up, 2f, LayerMask.GetMask("Walls", "Destructible")) ? 0 : 1;

        movementDirection = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    void PlayAnim()
    {
        if (Mathf.Abs(movementDirection.x) > Mathf.Abs(movementDirection.y))
        {
            if (movementDirection.x < 0)
            {
                sm.GetAnimator().Play("FBP3_Left");
            }
            else
            {
                sm.GetAnimator().Play("FBP3_Right");
            }
        }
        else
        {
            if (movementDirection.y < 0)
            {
                sm.GetAnimator().Play("FBP3_Forward");
            }
            else
            {
                sm.GetAnimator().Play("FBP3_Backward");
            }
        }
    }

    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();

        sm.GetRigidbody2D().MovePosition(sm.transform.position + (Vector3) movementDirection * sm.GetMovementSpeed() * Time.fixedDeltaTime);
    }

    bool shootDirectionHorizontal;

    void Shoot()
    {
        sm.GetAudioCall().CallAudioClip("Shoot1");

        shootDirectionHorizontal = !shootDirectionHorizontal;

        if (shootDirectionHorizontal)
        {
            Object.Instantiate(sm.GetProjectile(), sm.transform.position, Quaternion.identity).
                GetComponent<EnemyBullet>()?.SetDirection(Vector2.right);
            Object.Instantiate(sm.GetProjectile(), sm.transform.position, Quaternion.identity).
                GetComponent<EnemyBullet>()?.SetDirection(Vector2.left);
        }
        else
        {
            Object.Instantiate(sm.GetProjectile(), sm.transform.position, Quaternion.identity).
                GetComponent<EnemyBullet>()?.SetDirection(Vector2.up);
            Object.Instantiate(sm.GetProjectile(), sm.transform.position, Quaternion.identity).
                GetComponent<EnemyBullet>()?.SetDirection(Vector2.down);
        }
    }
}
