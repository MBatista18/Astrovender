using UnityEngine;

public class ChargingBossStateInitial : StateBase
{
    ChargingBossSM sm;

    public ChargingBossStateInitial(StateMachineBase _sm) : base(_sm)
    {
        sm = (ChargingBossSM)_sm;
    }

    float movingTimer;
    public override void thisStart()
    {
        base.thisStart();
        movingTimer = 3f;

        sm.GetAnimator().Play("ChargingBossInitial");
    }

    public override void thisUpdate()
    {
        base.thisUpdate();
        movingTimer -= Time.deltaTime;

        if (movingTimer <= 0 ||
            (Physics2D.BoxCast((sm.transform.position + ((Vector3)new Vector3(0, -1) * .01f)), Vector3.one * sm.GetBoxCollider2D().size.x, 0f, Vector2.zero, 0f,
            LayerMask.GetMask("Walls"))))
        {
            sm.ChangeState(sm.GetStateBurrow());
        }
    }
    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();

        sm.GetRigidbody2D().linearVelocity = -Vector2.up * sm.GetBurrowingSpeed();
    }
    public override void thisEnd()
    {
        base.thisEnd();

        sm.GetRigidbody2D().linearVelocity = Vector2.zero;
    }
}
