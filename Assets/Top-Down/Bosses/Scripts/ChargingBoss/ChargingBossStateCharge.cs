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

        sm.GetAnimator().Play(movementDirection < 0 ? "ChargingBossChargeDown" : "ChargingBossChargeUp");
    }

    public override void thisUpdate()
    {
        base.thisUpdate();
        runningTimer -= Time.deltaTime;

        RaycastHit2D a1 = Physics2D.BoxCast((sm.transform.position + ((Vector3)new Vector3(0, movementDirection) * (sm.GetBoxCollider2D().size.y/2)))
            , new Vector3(sm.GetBoxCollider2D().size.x, .1f), 0f, Vector2.zero, 0f,
            LayerMask.GetMask("Walls"));

        if (runningTimer <= 0 || 
            a1)
        {
            Debug.Log("COLLISION AT " + a1.point);

            sm.ChangeState(sm.GetStateStunned());
        }

        RaycastHit2D a = Physics2D.BoxCast((sm.transform.position + ((Vector3)new Vector3(0, movementDirection) * .1f)), 
            new Vector3(sm.GetBoxCollider2D().size.x + .1f, sm.GetBoxCollider2D().size.y + .1f), 0f, Vector2.zero, 0f,
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
