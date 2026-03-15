using UnityEngine;

public class ShielderRobotSM : EnemySM
{
    [SerializeField] float chargeTime = .5f;
    public float GetChargeTime() { return chargeTime; }
    [SerializeField] float rushSpeed = 4f;
    public float GetRushSpeed() { return rushSpeed; }

    ShielderRobotStateCharge stateCharge;
    public ShielderRobotStateCharge GetStateCharge() { return stateCharge; }
    ShielderRobotStateKnockback stateKnockback;
    public ShielderRobotStateKnockback GetStateKnockback() { return stateKnockback; }
    ShielderRobotStateRush stateRush;
    public ShielderRobotStateRush GetStateRush() { return stateRush; }
    ShielderRobotStatePatrol statePatrol;
    public override void InstantiateStates()
    {
        base.InstantiateStates();
        stateCharge = new ShielderRobotStateCharge(this);
        stateKnockback = new ShielderRobotStateKnockback(this);
        stateRush = new ShielderRobotStateRush(this);

        statePatrol = new ShielderRobotStatePatrol(this);
    }

    public override StateBase InitialState()
    {
        return statePatrol;
    }

    public override void InstantiateValues()
    {
        base.InstantiateValues();
    }

    public override StateBase AttackState()
    {
        return GetStateCharge();
    }

    public void CallKnockback()
    {
        if (GetCurrentState() == stateRush)
        {
            ChangeState(stateKnockback);
        }
    }

    public override void TakeDamage(int damageAmount)
    {
        base.TakeDamage(damageAmount);
    }

    Animator animator;
    public Animator GetAnimator() { return animator; }

    SpriteRenderer sprite;
    public void SwapFacingDirection(Vector3 facingPos)
    {
        switch (facingPos.x > transform.position.x)
        {
            case true:
                sprite.flipX = true;
                base.SetFacingDirection(AstrovenderStructs.facingDirection.right);
                break;
            case false:
                sprite.flipX = false;
                base.SetFacingDirection(AstrovenderStructs.facingDirection.left);
                break;
        }
    }

    public override void InstantiateComponents()
    {
        base.InstantiateComponents();
        animator = GetComponent<Animator>();
        sprite = transform.Find("Sprite")?.GetComponent<SpriteRenderer>();
    }
}
