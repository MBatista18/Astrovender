using UnityEngine;

public class BombTankBossSM : EnemySM
{
    [SerializeField] GameObject bomb;
    public GameObject GetBomb() { return bomb; }

    Animator animator;
    public Animator GetAnimator() { return animator; }

    public override void InstantiateComponents()
    {
        base.InstantiateComponents();

        animator = GetComponent<Animator>();
    }

    BombTankStateMove stateMove;
    public BombTankStateMove GetStateMove() { return stateMove; }
    
    BombTankStatePause statePause;
    public override StateBase InitialState()
    {
        return statePause;
    }

    BombTankStateRush stateRush;
    public override StateBase AttackState()
    {
        return stateRush;
    }

    public override void InstantiateStates()
    {
        base.InstantiateStates();

        stateMove = new BombTankStateMove(this);
        statePause = new BombTankStatePause(this);
        stateRush = new BombTankStateRush(this);
    }

    Vector3 lastPosition;

    public override void InstantiateValues()
    {
        base.InstantiateValues();
        lastPosition = transform.position;
    }

    public override void UpdateFunctions()
    {
        base.UpdateFunctions();

        float diffX = transform.position.x - lastPosition.x;
        float diffY = transform.position.y - lastPosition.y;

        if (Mathf.Abs(diffX) > Mathf.Abs(diffY))
        {
            if (diffX > 0)
            {
                animator.Play("BombTankRight");
            }

            if (diffX < 0)
            {
                animator.Play("BombTankLeft");
            }
        }
        else
        {
            if (diffY > 0)
            {
                animator.Play("BombTankUp");
            }

            if (diffY < 0)
            {
                animator.Play("BombTankDown");
            }
        }

        lastPosition = transform.position;
    }
}
