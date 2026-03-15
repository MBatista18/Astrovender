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
    public override void InstantiateStates()
    {
        base.InstantiateStates();
        stateCharge = new ShielderRobotStateCharge(this);
        stateKnockback = new ShielderRobotStateKnockback(this);
        stateRush = new ShielderRobotStateRush(this);
    }

    public override void InstantiateValues()
    {
        base.InstantiateValues();
        if (Random.Range(0,10) < 5) 
        {
            if (Random.Range(0, 10) < 5)
            {
                base.SetFacingDirection(AstrovenderStructs.facingDirection.left);
            }
            else
            {
                base.SetFacingDirection(AstrovenderStructs.facingDirection.right);
            }
        }
        else
        {
            if (Random.Range(0, 10) < 5)
            {
                base.SetFacingDirection(AstrovenderStructs.facingDirection.up);
            }
            else
            {
                base.SetFacingDirection(AstrovenderStructs.facingDirection.down);
            }
        }
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
}
