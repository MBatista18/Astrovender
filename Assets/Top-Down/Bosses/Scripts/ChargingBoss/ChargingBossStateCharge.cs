using UnityEngine;

public class ChargingBossStateCharge : StateBase
{
    ChargingBossSM sm;

    public ChargingBossStateCharge(StateMachineBase _sm) : base(_sm)
    {
        sm = (ChargingBossSM)_sm;
    }

    float runningTimer;

    float movementDirection;

    public override void thisStart()
    {
        base.thisStart();
        runningTimer = 4f;

        movementDirection = sm.facingDirection == AstrovenderStructs.facingDirection.down ? -1 : 1;

        sm.GetAnimator().Play("ChargingBossCharge");
    }

    public override void thisUpdate()
    {
        base.thisUpdate();
        runningTimer -= Time.deltaTime;

        if (runningTimer <= 0 || 
            (Physics2D.BoxCast((sm.transform.position + ((Vector3)new Vector3(0,movementDirection) * .1f)), Vector3.one * sm.GetBoxCollider2D().size.x, 0f, Vector2.zero, 0f, 
            LayerMask.GetMask("Walls"))))
        {
            sm.ChangeState(sm.GetStateStunned());
        }

        RaycastHit2D a = Physics2D.BoxCast((sm.transform.position + ((Vector3)new Vector3(0, movementDirection) * .1f)), Vector3.one * sm.GetBoxCollider2D().size.x, 0f, Vector2.zero, 0f,
            LayerMask.GetMask("Destructible"));

        if (a)
        {
            a.collider.gameObject.GetComponent<Destructible>()?.CallDestroy();
        }
    }

    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();

        sm.GetRigidbody2D().linearVelocity = Vector2.up * movementDirection * sm.GetMovementSpeed();
    }

    public override void thisEnd()
    {
        base.thisEnd();

        sm.GetRigidbody2D().linearVelocity = Vector2.zero;
    }
}
