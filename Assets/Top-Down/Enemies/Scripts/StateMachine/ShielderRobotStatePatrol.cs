using UnityEngine;

public class ShielderRobotStatePatrol : EnemyStatePatrol
{
    ShielderRobotSM sm;

    public ShielderRobotStatePatrol (StateMachineBase _sm) : base(_sm)
    {
        sm = (ShielderRobotSM)_sm;
    }
    public override void thisStart()
    {
        base.thisStart();

        sm.GetAnimator().Play("ShielderBotIdle");
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        sm.SwapFacingDirection(base.GetTargetPosition());
    }
}
