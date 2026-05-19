using UnityEngine;

public class EelStatePatrol : StateBase
{
    EelSM sm;

    public EelStatePatrol(StateMachineBase _sm) : base(_sm)
    {
        sm = (EelSM)_sm;
    }

    float delayAttack;

    public override void thisStart()
    {
        base.thisStart();

        sm.SetMoveDirection(sm.GetRandomDiagonalDirection());
        timer = sm.patrolChangeTime;
        delayAttack = Random.Range(.7f, 2f);
    }

    float timer;

    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();

        sm.GetRigidbody2D().linearVelocity =
            sm.GetMoveDirection() * sm.GetMovementSpeed();
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        sm.UpdateDirectionLockTimer();

        Vector2 dir = sm.GetMoveDirection();

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = sm.patrolChangeTime;
            sm.SetMoveDirection(sm.GetNewRandomDiagonalDirection(sm.GetMoveDirection()));
        }

        Vector3 scale = sm.originalScale;

        if (dir.x > 0)
            scale.x = -Mathf.Abs(scale.x);
        else if (dir.x < 0)
            scale.x = Mathf.Abs(scale.x);

        sm.transform.localScale = scale;

        if (sm.IsNearBoundsEdge() && sm.CanChangeDirection())
        {
            timer = sm.patrolChangeTime;

            sm.SetMoveDirection(sm.GetBoundsCorrectedDirection());
            sm.LockDirection();
        }

        //if (Vector2.Distance(sm.GetPlayerTransform().position, sm.transform.position) <= sm.GetDetectionRadius())
        
        if (delayAttack <= 0)
        {
            if (Vector2.Distance(sm.GetPlayerTransform().position, sm.transform.position) > 8) { return; }

            sm.ChangeState(sm.AttackState());
            return;
        }
        else
        {
            delayAttack -= Time.deltaTime;
        }
    }
}
