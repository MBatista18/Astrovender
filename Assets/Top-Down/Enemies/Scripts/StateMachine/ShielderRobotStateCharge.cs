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
        sm.GetAnimator().Play("ShielderBotCharge");
        timer = sm.GetChargeTime();
    }

    public override void thisUpdate()
    {
        base.thisUpdate();
        sm.SwapFacingDirection(AssetCall.instance.playerSM.transform.position);
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            sm.ChangeState(sm.GetStateRush());
        }
    }
}
