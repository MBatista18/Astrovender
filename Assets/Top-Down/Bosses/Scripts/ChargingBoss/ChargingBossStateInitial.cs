using UnityEngine;

public class ChargingBossStateInitial : StateBase
{
    ChargingBossSM sm;

    public ChargingBossStateInitial(StateMachineBase _sm) : base(_sm)
    {
        sm = (ChargingBossSM)_sm;
    }

    float movingTimer;
    float idleTimer = 2f;
    public override void thisStart()
    {
        base.thisStart();
        movingTimer = 3f;

        sm.GetAnimator().Play("ChargingBossInitial");
    }

    public override void thisUpdate()
    {
        base.thisUpdate();
        if (idleTimer > 0)
        {
            idleTimer -= Time.deltaTime;
            return;
        }

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

        if (idleTimer > 0) { return; }

        sm.GetRigidbody2D().linearVelocity = -Vector2.up * sm.GetBurrowingSpeed() *.75f;
    }
    public override void thisEnd()
    {
        base.thisEnd();

        sm.GetRigidbody2D().linearVelocity = Vector2.zero;
    }
}
