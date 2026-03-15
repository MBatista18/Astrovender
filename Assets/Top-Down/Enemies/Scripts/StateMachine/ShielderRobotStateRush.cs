using UnityEngine;

public class ShielderRobotStateRush : StateBase
{
    ShielderRobotSM sm;

    public ShielderRobotStateRush(StateMachineBase _sm) : base(_sm)
    {
        sm = (ShielderRobotSM)_sm;
    }

    Vector2 playerDirection;

    public override void thisStart()
    {
        base.thisStart();
        sm.GetAnimator().Play("ShielderBotRush");
        playerDirection = (AssetCall.instance.playerSM.transform.position - sm.transform.position).normalized;

        sm.ShieldKnockbackRush(true);
    }

    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();

        sm.GetRigidbody2D().linearVelocity = playerDirection * sm.GetRushSpeed();

        if (Physics2D.BoxCast((sm.transform.position + ((Vector3)playerDirection * .1f)), Vector3.one, 0f, Vector2.zero, 0f, LayerMask.GetMask("Walls", "Destructible")))
        {
            sm.CallKnockback();
        }
    }

    public override void thisEnd()
    {
        base.thisEnd();
        sm.GetRigidbody2D().linearVelocity = Vector2.zero;
        sm.GetStateKnockback().direction = -playerDirection;

        sm.ShieldKnockbackRush(false);
    }
}
