using UnityEngine;

public class SquidStateIdle : StateBase
{
    SquidBossSM sm;

    public SquidStateIdle (StateMachineBase _sm) : base (_sm)
    {
        sm = (SquidBossSM)_sm;
        horizontalMovement = true;
    }

    bool horizontalMovement;

    float timer;
    public override void thisStart()
    {
        base.thisStart();
        timer = Random.Range(1f, 4f);
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            sm.ChangeState(horizontalMovement ? sm.GetStateHorizontal() : sm.GetStateVertical());
        }
    }

    public override void thisEnd()
    {
        base.thisEnd();
        horizontalMovement = !horizontalMovement;
    }
}
