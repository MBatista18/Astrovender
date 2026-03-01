using UnityEngine;

public class PlayerStateIdle : StateBase
{
    PlayerStateMachine sm;

    public PlayerStateIdle(StateMachineBase _sm) : base(_sm)
    {
        sm = (PlayerStateMachine)_sm;
    }

    public override void thisStart()
    {
        base.thisStart();

       // Debug.Log("Check for controller returns " + (sm.GetAnimationController() != null));
        sm.GetAnimationController().SetAnimatorState(PlayerAnimationController.AnimatorState.Idle);
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        if (!sm.GetMovement().Equals(Vector2.zero))
        {
            sm.ChangeState(sm.GetStateMove());
        }
    }
}
