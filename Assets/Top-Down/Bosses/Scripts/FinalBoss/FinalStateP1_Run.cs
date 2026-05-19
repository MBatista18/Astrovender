using UnityEngine;

public class FinalStateP1_Run : StateBase
{
    FinalBossSM sm;

    public FinalStateP1_Run(StateMachineBase _sm) : base(_sm)
    {
        sm = (FinalBossSM)_sm;
        changeDirectionTimer = 1f;

        int a = Random.Range(0, 4);

        switch (a)
        {
            case 0:
                facingDirection = Vector2.down;
                return;
            case 1:
                facingDirection = Vector2.up;
                return;
            case 2:
                facingDirection = Vector2.left;
                return;
        }
        facingDirection = Vector2.right;
    }

    float runningTime;

    public override void thisStart()
    {
        base.thisStart();
        runningTime = Random.Range(2f, 4f);

        RecalculateDirection();
    }

    float changeDirectionTimer;

    public override void thisUpdate()
    {
        base.thisUpdate();

        runningTime -= Time.deltaTime;

        if (runningTime <= 0)
        {
            sm.ChangeState(sm.stateP1_Pause);
        }

        changeDirectionTimer -= Time.deltaTime;

        if (changeDirectionTimer <= 0)
        {
            changeDirectionTimer = Random.Range(0.75f, 1.5f);
            RecalculateDirection();
        }
    }

    Vector2 facingDirection;

    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();

        sm.GetRigidbody2D().MovePosition(sm.transform.position + (Vector3)facingDirection * sm.GetMovementSpeed() * Time.fixedDeltaTime);

        RaycastHit2D ray = Physics2D.BoxCast(sm.transform.position + (Vector3)(facingDirection * sm.GetCollider2D().size.x / 2),
            Mathf.Abs(facingDirection.x) > Mathf.Abs(facingDirection.y) ? new Vector2(.1f, sm.GetCollider2D().size.y) : new Vector2(sm.GetCollider2D().size.x, .1f),
            0f, Vector2.zero, 0f, LayerMask.GetMask("Destructible", "Walls"));

        if (ray)
        {
            RecalculateDirection();
        }
    }

    void RecalculateDirection()
    {
        facingDirection = Mathf.Abs(facingDirection.x) > Mathf.Abs(facingDirection.y) ?
            new Vector2(0, Random.Range(0, 10) < 6 ? -1 : 1) : new Vector2(Random.Range(0, 10) < 6 ? -1 : 1, 0);

        if (Mathf.Abs(facingDirection.x) > Mathf.Abs(facingDirection.y))
        {
            if (facingDirection.x >= 0)
            {
                RaycastHit2D stopCheck = Physics2D.BoxCast(sm.transform.position + (Vector3)(Vector2.right * sm.GetCollider2D().size.x / 2),
                    new Vector2(.1f, sm.GetCollider2D().size.y),
                    0f, Vector2.zero, 0f, LayerMask.GetMask("Destructible", "Walls"));

                if (stopCheck) { Debug.Log("True1"); facingDirection = new Vector2(-1, 0); }
            }
            else
            {
                RaycastHit2D stopCheck = Physics2D.BoxCast(sm.transform.position + (Vector3)(Vector2.left * sm.GetCollider2D().size.x / 2),
                    new Vector2(.1f, sm.GetCollider2D().size.y),
                    0f, Vector2.zero, 0f, LayerMask.GetMask("Destructible", "Walls"));

                if (stopCheck) { Debug.Log("True2"); facingDirection = new Vector2(1, 0); }
            }

            sm.GetAnimator().Play(facingDirection.x >= 0 ? "FBP1_Right" : "FBP1_Left");
        }
        else
        {
            if (facingDirection.x >= 0)
            {
                RaycastHit2D stopCheck = Physics2D.BoxCast(sm.transform.position + (Vector3)(Vector2.up * sm.GetCollider2D().size.y / 2),
                    new Vector2(sm.GetCollider2D().size.x, .1f),
                    0f, Vector2.zero, 0f, LayerMask.GetMask("Destructible", "Walls"));

                if (stopCheck) { Debug.Log("True3"); facingDirection = new Vector2(0, -1); }
            }
            else
            {
                RaycastHit2D stopCheck = Physics2D.BoxCast(sm.transform.position + (Vector3)(Vector2.down * sm.GetCollider2D().size.y / 2),
                    new Vector2(sm.GetCollider2D().size.x, .1f),
                    0f, Vector2.zero, 0f, LayerMask.GetMask("Destructible", "Walls"));

                if (stopCheck) { Debug.Log("True4"); facingDirection = new Vector2(0, 1); }
            }

            sm.GetAnimator().Play(facingDirection.y >= 0 ? "FBP1_Backward" : "FBP1_Forward");
        }
    }
}
