using UnityEngine;

public class BombBossStateVulnerable : StateBase
{
    BombBossSM sm;
    public BombBossStateVulnerable(StateMachineBase _sm) : base(_sm)
    {
        sm = (BombBossSM)_sm;
    }

    float vulnTimer;
    public override void thisStart()
    {
        base.thisStart();

        vulnTimer = Random.Range(2f, 3f);

        Debug.Log("Vulnerable");
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        vulnTimer -= Time.deltaTime;

        if (vulnTimer <= 0)
        {
            sm.ChangeState(sm.InitialState()); // Go back to hidden state
        }
    }
}
