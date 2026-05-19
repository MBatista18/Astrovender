using UnityEngine;

public class FinalStateP1_Pause : StateBase
{
    FinalBossSM sm;

    public FinalStateP1_Pause(StateMachineBase _sm) : base(_sm)
    {
        sm = (FinalBossSM)_sm;
    }

    float timer;
    float MAXtimer = 1f;

    bool firstPause = true;

    public override void thisStart()
    {
        base.thisStart();
        
        if (Random.Range(0, 10) < 8)
        {
            timer = MAXtimer *.33f;


            if (firstPause) { firstPause = false; return; }

            sm.GetAudioCall().CallAudioClip("PlaceLaser");

            Object.Instantiate(sm.GetLaser(), sm.transform.position, Quaternion.identity);
        }
        else
        {
            timer = MAXtimer;
        }
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            sm.ChangeState(sm.stateP1_Run);
        }
    }
}
