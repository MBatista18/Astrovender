using UnityEngine;

public class EnemyStateHurt : StateBase
{
    // short buffer state after an enemy is hit to give the attack some impact
        // note: may wanna modify this for different attacks (e.g. bullets have 0 hurt time, melee has 0.2 seconds hurt time, bomb has 0.5 seconds hurt time, etc.)

    EnemySM sm;
    public EnemyStateHurt (StateMachineBase _sm) : base(_sm)
    {
        sm = (EnemySM)_sm;
    }

    float timer;

    public override void thisStart()
    {
        base.thisStart();

        timer = .2f;
    }

    public override void thisUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            sm.ChangeState(sm.AttackState());
        }

        base.thisUpdate();
    }
}
