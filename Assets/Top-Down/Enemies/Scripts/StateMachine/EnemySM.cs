using UnityEngine;

public class EnemySM : StateMachineBase // this is the base for the enemy state machine. all enemies will be built off of this statemachine
{
    [Header("Damage Indicator")]
    FlashOnHit flashOnHit;
    [SerializeField] GameObject deathEffectPrefab;
    public GameObject GetDeathEffect() { return deathEffectPrefab; }

    [Header("Health Values")]
    [SerializeField] int maxHealth;
    int health;
    public int GetHealth() { return health; }
    public void SetHealth(int val) { health = val; }
    public int GetMaxHealth() { return maxHealth; }

    [SerializeField] float damageReactionTime = .2f;
    public float GetDamageReactionTime() { return damageReactionTime; }

    [SerializeField] bool reactToDamage = true;
    public void SetReactToDamage(bool a) { reactToDamage = a; }

    [SerializeField] bool shielded;
    bool ShieldProtects(Vector3 otherPos)
    {
        bool didDefend = false;

        if (shielded)
        {
            Vector3 difference = transform.position - otherPos;

            float axisDiff = Mathf.Abs(Mathf.Abs(difference.x) - Mathf.Abs(difference.y));

            float chosenAxis = 0; // 0 = x, 1 = y, 2 = both

            if (axisDiff >= .1) { chosenAxis = Mathf.Abs(difference.x) > Mathf.Abs(difference.y) ? 0 : 1; } else { chosenAxis = 2; }

            if (chosenAxis == 0 || chosenAxis == 2) // x check
            {
                switch (facingDirection)
                {
                    case AstrovenderStructs.facingDirection.left:

                        if (difference.x >= 0) { Debug.Log("Counter attack coming left"); didDefend = true; }

                        break;
                    case AstrovenderStructs.facingDirection.right:

                        if (difference.x <= 0) { Debug.Log("Counter attack coming right"); didDefend = true; }

                        break;
                }
            }
            else if (chosenAxis == 1 || chosenAxis == 2) // y check
            {
                switch (facingDirection)
                {
                    case AstrovenderStructs.facingDirection.down:

                        if (difference.y >= 0) { Debug.Log("Counter attack coming from below"); didDefend = true; }

                        break;
                    case AstrovenderStructs.facingDirection.up:

                        if (difference.y <= 0) { Debug.Log("Counter attack coming from above"); didDefend = true; }

                        break;
                }
            }
        }

        return didDefend;
    }
    
    public virtual void TakeDamage(int damageAmount, Vector3 attackerPos)
    {
        if (ShieldProtects(attackerPos))
        {
            shield.PlayHit();
            return;
        }

        TakeDamage(damageAmount);
    }

    public virtual void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (reactToDamage && GetCurrentState() != stateHurt) { ChangeState(stateHurt); PainReactions(); }
        // changes enemy to state hurt; check for if the enemy is already in hurt state to prevent player from just hitting enemy relentlessly
        if (health <= 0)
        {
            ChangeState(DeathState());
        }
    }

    public virtual void PainReactions()
    {
        flashOnHit?.FlashRed();
        return;
    }

    [Header("Movement Values")]

    [SerializeField] float movementSpeed = 2f;
    public float GetMovementSpeed() { return movementSpeed; }

    [SerializeField] float patrolRadius = 3f;
    public float GetPatrolRadius() { return patrolRadius; }

    [SerializeField] float detectionRadius = 5f;
    public float GetDetectionRadius() { return detectionRadius; }

    [Header("Death Effects")]
    [SerializeField] bool dropsCoinsGems = true;

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

    EnemyShield shield;
    public EnemyShield GetShield() { return shield; }

    public override void InstantiateComponents()
    {
        base.InstantiateComponents();
        playerTransform = AssetCall.instance.playerSM.transform;
        rb2D = GetComponent<Rigidbody2D>();
        shield = GetComponentInChildren<EnemyShield>();
        flashOnHit = GetComponent<FlashOnHit>();
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
        Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

        if (dropsCoinsGems)
        {
            if (Random.Range(0, 10) <= 7)
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
        }

        Destroy(this.gameObject);
        return base.DeathState();
    }

}
