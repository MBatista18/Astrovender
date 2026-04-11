using UnityEngine;

public class BatEnemyStateAttack : StateBase
{
    BatEnemySM sm;

    public BatEnemyStateAttack(StateMachineBase _sm) : base(_sm)
    {
        sm = (BatEnemySM)_sm;
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        if (Vector2.Distance(sm.GetPlayerTransform().position, sm.transform.position) > sm.GetDetectionRadius() * 1.5f)
        {
            sm.ChangeState(sm.InitialState());
            return;
        }

        Vector2 dir = sm.GetDiagonalOnlyDirection(sm.GetPlayerTransform().position);
        sm.SetMoveDirection(dir);
    }
}
