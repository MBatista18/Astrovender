using UnityEngine;

public class EnemySM : StateMachineBase // this is the base for the enemy state machine. all enemies will be built off of this statemachine
{
    [Header("Enemy Base Values")]
    [SerializeField] int health, maxHealth;
    public float GetHealth() { return health; }
    public float GetMaxHealth() { return maxHealth; }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] float movementSpeed = 2f;
    public float GetMovementSpeed() { return movementSpeed; }

    [SerializeField] float detectionRadius = 5f;
    public float GetDetectionRadius() { return detectionRadius; }

    [Header("Components")]

    Transform playerTransform;
    public Transform GetPlayerTransform() { return playerTransform; }

    public override void InstantiateComponents()
    {
        base.InstantiateComponents();
        playerTransform = FindFirstObjectByType<PlayerMovement>().transform;
    }

    [Header("States")]

    public EnemyStatePatrol statePatrol; // enemy's first state will be patroling around the game environment.
    public override StateBase InitialState()
    {
        return statePatrol;
    }

    public override void InstantiateStates()
    {
        base.InstantiateStates();
        statePatrol = new EnemyStatePatrol(this);
    }

    public virtual StateBase AttackState()
    {
        return statePatrol;
    }

    public override StateBase DeathState()
    {
        Destroy(this.gameObject);
        return base.DeathState();
    }
}
