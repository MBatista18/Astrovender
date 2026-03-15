using UnityEngine;

public class ShielderRobotStateKnockback : StateBase
{
    ShielderRobotSM sm;

    public ShielderRobotStateKnockback(StateMachineBase _sm) : base(_sm)
    {
        sm = (ShielderRobotSM)_sm;
    }

    public Vector2 direction;

    float knockbackTime;
    float MAXknockbackTime = 2f;

    public override void thisStart()
    {
        base.thisStart();
        sm.GetAnimator().Play("ShielderBotKnockback");
        knockbackTime = MAXknockbackTime;
    }

    public float GetMovement()
    {
        float movementSpeed = sm.GetMovementSpeed();
        float multiplier = 0;
        if (knockbackTime/MAXknockbackTime > 0.5f)
        {
            Debug.Log("y = -sin((1 - " + (knockbackTime / MAXknockbackTime) + ") ^ 2) / .16f + 1");
            multiplier = -Mathf.Sin((Mathf.Pow(Mathf.Clamp01(1-(knockbackTime / MAXknockbackTime)), 2) / .16f)) + 1;
            Debug.Log(multiplier + " = -sin((1 - " + (knockbackTime / MAXknockbackTime) + ") ^ 2) / .16f + 1");
        }

        //Debug.Log("Multiplier = " + multiplier);

        return movementSpeed * multiplier;
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        knockbackTime -= Time.deltaTime;

        if (knockbackTime <= 0)
        {
            sm.ChangeState(sm.InitialState());
        }
    }

    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();

        sm.GetRigidbody2D().linearVelocity = direction * GetMovement();
    }

    public override void thisEnd()
    {
        base.thisEnd();
        sm.GetRigidbody2D().linearVelocity = Vector2.zero;
    }
}
