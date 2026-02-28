using UnityEngine;

public class PlayerStateMove : StateBase
{
    PlayerStateMachine sm;

    public PlayerStateMove(StateMachineBase _sm) : base(_sm)
    {
        sm = (PlayerStateMachine)_sm;
    }

    public override void thisStart()
    {
        base.thisStart();

        sm.GetAnimationController().SetAnimatorState(PlayerAnimationController.AnimatorState.Walking);
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        if (sm.GetMovement().Equals(Vector2.zero))
        {
            sm.ChangeState(sm.InitialState());
        }
    }

    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();

        sm.GetRB2d().linearVelocity = sm.GetMovement() * sm.GetSpeed();
    }

    public override void thisEnd()
    {
        base.thisEnd();
        sm.GetRB2d().linearVelocity = Vector2.zero; // rests velocity at the end of the state to prevent rb velocity continuing into other states
    }
}
