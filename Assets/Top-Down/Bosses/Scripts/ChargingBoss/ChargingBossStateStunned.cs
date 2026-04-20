using UnityEngine;

public class ChargingBossStateStunned : StateBase
{
    ChargingBossSM sm;

    public ChargingBossStateStunned(StateMachineBase _sm) : base(_sm)
    {
        sm = (ChargingBossSM)_sm;
    }

    float timer;

    public override void thisStart()
    {
        base.thisStart();
        sm.GetRockSlideManager().CallRockslide();
        timer = sm.GetStunTime();

        sm.GetAudioCall().CallAudioClip("Crash");

        sm.GetAnimator().Play("ChargingBossStun");
    }

    public override void thisUpdate()
    {
        base.thisUpdate();
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            sm.ChangeState(sm.GetStateBurrow());
        }
    }

}
