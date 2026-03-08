using UnityEngine;

public class GunBossStateMove : StateBase
{
    GunBossSM sm;

    Vector2 moveDirection;
    float moveDuration;

    int beginningHealth;

    public GunBossStateMove(StateMachineBase _sm) : base(_sm)
    {
        sm = (GunBossSM)_sm;

        moveDirection = Vector2.up; // these values are set up here so the enemy rushes up the screen at the start of the fight
        sm.SetFacingDirection(AstrovenderStructs.facingDirection.up);


        moveDuration = 7f; // rather than setting this in start, this value is set here so changing states doesn't cause the timer to reset, important for when
                                // it changes to idling while shooting at the player
    }

    float attackTimer;

    void RecalculateAttackTime()
    {
        attackTimer = Random.Range(Mathf.Clamp(sm.GetHealth() / sm.GetMaxHealth(), 0.3f, 1f), 3f); // attacks more frequently as its health goes down

        if (wasAttacked) 
        { 
            Mathf.Clamp(attackTimer, 2, 2.5f);
        } // if the enemy is already attacking at the beginning of the state, add a little delay for the next attack
       }

    public void SetWasAttackedTrue() { wasAttacked = true; } // this is so that, if the enemy is attacked, it'll start shooting while moving at the beginning of this state as a defensive maneuver
    bool wasAttacked;
    Coroutine shootingCoroutine;

    public override void thisStart()
    {
        base.thisStart();

        if (wasAttacked) { shootingCoroutine = sm.StartCoroutine(sm.shoot()); } // if this enemy was previously stunned and now gets to attack, it'll shoot at the beginning of this movement
        
        RecalculateAttackTime();

        wasAttacked = false;
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        moveDuration -= Time.deltaTime;

        bool check = Physics2D.BoxCast(sm.transform.position + ((Vector3)moveDirection * .1f), new Vector2(1.7f, 1.7f), 0f, Vector2.zero, 0f, LayerMask.GetMask("Walls"));

        if (moveDuration <= 0 || check) // checks if the boss is either about to hit a wall or has moved for longer than 7 seconds
        {
            ChangeDirection();
            moveDuration = 7;
        }

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            sm.ChangeState(sm.InitialState());
        }
    }

    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();

        sm.GetRigidbody2D().linearVelocity = moveDirection * sm.GetMovementSpeed();
    }

    void ChangeDirection() // changes the boss's movement counterclockwise
    {
        if (moveDirection == Vector2.right)
        {
            moveDirection = Vector2.up;
            sm.SetFacingDirection(AstrovenderStructs.facingDirection.up);
        }
        else if (moveDirection == Vector2.up)
        {
            moveDirection = Vector2.left;
            sm.SetFacingDirection(AstrovenderStructs.facingDirection.left);
        }
        else if (moveDirection == Vector2.left)
        {
            moveDirection = Vector2.down;
            sm.SetFacingDirection(AstrovenderStructs.facingDirection.down);
        }
        else if (moveDirection == Vector2.down)
        {
            moveDirection = Vector2.right;
            sm.SetFacingDirection(AstrovenderStructs.facingDirection.right);
        }

        RecalculateAttackTime();
    }

    public override void thisEnd()
    {
        base.thisEnd();

        if (shootingCoroutine != null)
        {
            sm.StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }

        sm.GetRigidbody2D().linearVelocity = Vector2.zero;
    }
}
