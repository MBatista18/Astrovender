using UnityEngine;

public class EnemySM : StateMachineBase // this is the base for the enemy state machine. all enemies will be built off of this statemachine
{
    [Header("Enemy Base Values")]
    [SerializeField] int maxHealth;
    int health;
    public float GetHealth() { return health; }
    public float GetMaxHealth() { return maxHealth; }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (GetCurrentState() != stateHurt) { ChangeState(stateHurt); } 
            // changes enemy to state hurt; check for if the enemy is already in hurt state to prevent player from just hitting enemy relentlessly

        if (health <= 0)
        {
            ChangeState(DeathState());
        }
    }

    [SerializeField] float movementSpeed = 2f;
    public float GetMovementSpeed() { return movementSpeed; }

    [SerializeField] float patrolRadius = 3f;
    public float GetPatrolRadius() { return patrolRadius; }

    [SerializeField] float detectionRadius = 5f;
    public float GetDetectionRadius() { return detectionRadius; }

    public override void InstantiateValues()
    {
        base.InstantiateValues();
        health = maxHealth;
    }

    [Header("Components")]

    Transform playerTransform;
    public Transform GetPlayerTransform() { return playerTransform; }

    Rigidbody2D rb2D;
    public Rigidbody2D GetRigidbody2D() { return rb2D; }

    public override void InstantiateComponents()
    {
        base.InstantiateComponents();
        playerTransform = AssetCall.instance.playerSM.transform;
        rb2D = GetComponent<Rigidbody2D>();
    }

    [Header("States")]
    EnemyStatePatrol statePatrol; // enemy's first state will be patroling around the game environment.
    public override StateBase InitialState()
    {
        return statePatrol;
    }

    EnemyStateHurt stateHurt;

    public override void InstantiateStates()
    {
        base.InstantiateStates();
        statePatrol = new EnemyStatePatrol(this);
        stateHurt = new EnemyStateHurt(this);
    }

    public virtual StateBase AttackState() // override this with whatever aggressive state this enemy has (e.g. chaser enemies go into their chase state)
    {
        return statePatrol;
    }

    public override StateBase DeathState()
    {
        if (Random.Range(0,10) <= 7) 
        { //spawn coin
            Instantiate(AssetCall.instance.coin, transform.position + (Vector3)Random.insideUnitCircle * .3f, Quaternion.identity);
        }
        else
        {
            if (Random.Range(0, 10) <= 3)
            { //spawn gem
                Instantiate(AssetCall.instance.gem, transform.position + (Vector3)Random.insideUnitCircle * .3f, Quaternion.identity);
            }
        }

        Destroy(this.gameObject);
        return base.DeathState();
    }
}
