using UnityEngine;

public class BombRobotStateAttack : StateBase
{
    BombRobotSM sm;

    public BombRobotStateAttack (StateMachineBase _sm) : base (_sm)
    {
        sm = (BombRobotSM)_sm;
    }

    public override void thisStart()
    {
        base.thisStart();

        sm.GetAnimator().Play("BombExploding");
        timer = sm.GetFuseTime();
    }

    float timer;

    public override void thisUpdate()
    {
        base.thisUpdate();

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Object.Instantiate(AssetCall.instance.explosion, sm.transform.position, Quaternion.identity);
            Object.Destroy(sm.gameObject);
        }
    }
}
