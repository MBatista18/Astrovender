using UnityEngine;

public class ShielderRobotStateCharge : StateBase
{
    ShielderRobotSM sm;

    public ShielderRobotStateCharge(StateMachineBase _sm) : base(_sm)
    {
        sm = (ShielderRobotSM)_sm;
    }

    float timer;

    public override void thisStart()
    {
        base.thisStart();
        timer = sm.GetChargeTime();
    }

    public override void thisUpdate()
    {
        base.thisUpdate();
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            sm.ChangeState(sm.GetStateRush());
        }
    }
}
