using UnityEngine;

public class EnemyStateKnockback : StateBase
{
    EnemySM sm;
    public EnemyStateKnockback(StateMachineBase _sm) : base(_sm)
    {
        sm = (EnemySM)_sm;
    }

    private Vector2 movementDir;
    private float time;
    private float fullTime;

    float movementSpeed = 1f;

    public void SetKnockback(Vector2 _movementDir, float _time)
    {
        movementDir = _movementDir;
        time = _time;
    }

    public override void thisStart()
    {
        base.thisStart();

        sm.GetAnimator().Play("Knockback");

        fullTime = time;
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        time -= Time.deltaTime;

        if (time <= 0)
        {
            sm.ChangeState(sm.InitialState());
        }
    }

    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();

        sm.GetRigidbody2D().linearVelocity = movementDir * (movementSpeed * Mathf.Clamp01(time / fullTime)); // has the player knockback progressively slow as the timer reaches 0
    }

    public override void thisEnd()
    {
        base.thisEnd();

        sm.GetRigidbody2D().linearVelocity = Vector2.zero; // rests velocity at the end of the state to prevent rb velocity continuing into other states
    }
}
