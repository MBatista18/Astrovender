using UnityEngine;

public class ChargingBossStateBurrowAlign : StateBase
{
    ChargingBossSM sm;

    public ChargingBossStateBurrowAlign(StateMachineBase _sm) : base(_sm)
    {
        sm = (ChargingBossSM)_sm;
    }

    float movementDirection;
    float timer;

    public override void thisStart()
    {
        base.thisStart();
        timer = sm.GetBurrowingTime();

        sm.GetBoxCollider2D().excludeLayers = LayerMask.GetMask("Walls");

        if (sm.GetFacingDirection() == AstrovenderStructs.facingDirection.down)
        {
            sm.SetFacingDirection(AstrovenderStructs.facingDirection.up);
            sm.transform.position -= Vector3.up * (sm.GetBoxCollider2D().size.y/2);
        }
        else if (sm.GetFacingDirection() == AstrovenderStructs.facingDirection.up)
        {
            sm.SetFacingDirection(AstrovenderStructs.facingDirection.down);
            sm.transform.position += Vector3.up * (sm.GetBoxCollider2D().size.y / 2);
        }

        sm.GetAnimator().Play("ChargingBossBurrowMove");
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            sm.ChangeState(sm.GetStateCharge());
        }

        float diffX = sm.transform.position.x - AssetCall.instance.playerSM.transform.position.x;

        if (diffX < -1)
        {
            movementDirection = 1;
        }
        else if (diffX > 1)
        {
            movementDirection = -1;
        }
        else
        {
            movementDirection = 0;
        }
    }

    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();

        sm.GetRigidbody2D().linearVelocity = Vector2.right * movementDirection * sm.GetBurrowingSpeed();
        sm.transform.position = new Vector3(Mathf.Clamp(sm.transform.position.x, sm.GetLeftX(), sm.GetRightX()), sm.transform.position.y, 0);
    }

    public override void thisEnd()
    {
        base.thisEnd();

        sm.GetRigidbody2D().linearVelocity = Vector2.zero;

        if (sm.GetFacingDirection() == AstrovenderStructs.facingDirection.down)
        {
            sm.transform.position -= Vector3.up * (sm.GetBoxCollider2D().size.y / 2);
        }
        else if (sm.GetFacingDirection() == AstrovenderStructs.facingDirection.up)
        {
            sm.transform.position += Vector3.up * (sm.GetBoxCollider2D().size.y / 2);
        }

        sm.GetBoxCollider2D().excludeLayers = LayerMask.GetMask();

        sm.GetSpriteRenderer().flipY = false;
    }
}
