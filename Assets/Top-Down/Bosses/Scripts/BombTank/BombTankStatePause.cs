using UnityEngine;

public class BombTankStatePause : StateBase
{
    BombTankBossSM sm;
    public BombTankStatePause(StateMachineBase _sm) : base(_sm)
    {
        sm = (BombTankBossSM)_sm;
    }

    float pauseTimer;
    public override void thisStart()
    {
        base.thisStart();

        pauseTimer = Random.Range(0.7f, 1.75f);

        Debug.Log("Pause");
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        pauseTimer -= Time.deltaTime;

        if (pauseTimer <= 0)
        {
            sm.ChangeState(sm.GetStateMove());
        }
    }
}
