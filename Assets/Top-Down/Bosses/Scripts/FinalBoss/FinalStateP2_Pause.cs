using UnityEngine;

public class FinalStateP2_Pause : StateBase
{
    FinalBossSM sm;

    public FinalStateP2_Pause(StateMachineBase _sm) : base(_sm)
    {
        sm = (FinalBossSM)_sm;
    }

    float timer;
    float MAXtimer = 5f;

    public override void thisStart()
    {
        base.thisStart();
        timer = MAXtimer;
        sm.GetAnimator().Play("FBP2_Stunned");
    }

    public override void thisUpdate()
    {
        base.thisUpdate();
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            sm.ChangeState(sm.stateP2_Rocket);
        }
    }
}
