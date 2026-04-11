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
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        Vector2 dir = sm.GetMoveDirection();

        if (dir.x > 0)
        {
            sm.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (dir.x < 0)
        {
            sm.transform.localScale = new Vector3(1, 1, 1);
        }

        if (sm.DidHitWall())
        {
            sm.SetMoveDirection(sm.GetNewRandomDiagonalDirection(sm.GetMoveDirection()));
            sm.ClearHitWall();
        }

        if (Vector2.Distance(sm.GetPlayerTransform().position, sm.transform.position) <= sm.GetDetectionRadius())
        {
            sm.ChangeState(sm.AttackState());
            return;
        }
    }
}