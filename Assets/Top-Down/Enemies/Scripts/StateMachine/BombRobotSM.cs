using UnityEngine;

public class BombRobotSM : EnemySM
{
    [SerializeField] float fuseTime;
    public float GetFuseTime() { return fuseTime; }

    Animator animator;
    public Animator GetAnimator() { return animator; }
    public override void InstantiateComponents()
    {
        base.InstantiateComponents();

        animator = GetComponent<Animator>();
    }

    BombRobotStateAttack stateAttack;

    public override StateBase AttackState()
    {
        return stateAttack;
    }

    BombRobotStatePatrol statePatrol;
    public override StateBase InitialState()
    {
        return statePatrol;
    }

    public override void InstantiateStates()
    {
        base.InstantiateStates();

        stateAttack = new BombRobotStateAttack(this);
        statePatrol = new BombRobotStatePatrol(this);
    }
}
