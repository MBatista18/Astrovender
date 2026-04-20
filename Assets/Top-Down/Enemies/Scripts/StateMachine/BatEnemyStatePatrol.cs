using UnityEngine;

public class BatEnemyStatePatrol : StateBase
{
    BatEnemySM sm;

    public BatEnemyStatePatrol(StateMachineBase _sm) : base(_sm)
    {
        sm = (BatEnemySM)_sm;
    }

    public override void thisStart()
    {
        base.thisStart();
        sm.SetMoveDirection(sm.GetRandomDiagonalDirection());
        timer = maxTimer;
    }

    float timer;
    float maxTimer = 2f; 
    
    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();
        sm.GetRigidbody2D().linearVelocity = sm.GetMoveDirection() * sm.GetMovementSpeed();
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        sm.UpdateDirectionLockTimer();
        Debug.Log("PATROLING AROUND");

        Vector2 dir = sm.GetMoveDirection();

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = maxTimer;
            sm.SetMoveDirection(sm.GetNewRandomDiagonalDirection(sm.GetMoveDirection()));
        }

        if (dir.x > 0)
            sm.transform.localScale = new Vector3(-1, 1, 1);
        else if (dir.x < 0)
            sm.transform.localScale = new Vector3(1, 1, 1);

        if (sm.IsNearBoundsEdge() && sm.CanChangeDirection())
        {
            timer = maxTimer;

            sm.SetMoveDirection(sm.GetBoundsCorrectedDirection());
            sm.LockDirection();

        }

        if (Vector2.Distance(sm.GetPlayerTransform().position, sm.transform.position) <= sm.GetDetectionRadius())
        {
            sm.ChangeState(sm.AttackState());
            return;
        }
    }
}