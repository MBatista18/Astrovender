using UnityEngine;

public class ChargingBossStateBurrow : StateBase
{
    ChargingBossSM sm;

    public ChargingBossStateBurrow(StateMachineBase _sm) : base (_sm)
    {
        sm = (ChargingBossSM)_sm;
    }

    float timer;

    public override void thisStart()
    {
        base.thisStart();
        timer = .2f;

        sm.GetAnimator().Play("ChargingBossBurrowing");
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        timer -= Time.deltaTime;

        if (timer <= 0) { sm.ChangeState(sm.GetStateBurrowAlign()); }
    }
}
