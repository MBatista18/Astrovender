using UnityEngine;

public class PlayerStateDead : StateBase
{
    PlayerStateMachine sm;

    public PlayerStateDead(StateMachineBase _sm) : base(_sm)
    {
        sm = (PlayerStateMachine) _sm;
    }

    public override void thisEnd()
    {
        base.thisEnd();

        // do something here;
    }
}
