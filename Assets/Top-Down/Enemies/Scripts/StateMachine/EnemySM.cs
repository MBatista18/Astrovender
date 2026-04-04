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
        if (deathEffectPrefab) { Instantiate(deathEffectPrefab, transform.position, Quaternion.identity); }

        if (dropsCoinsGems)
        {
            OnDeathCall();
        }

        Destroy(this.gameObject);
        return base.DeathState();
    }

    void OnDeathCall()
    {
        /*if (Random.Range(0, 10) <= 7)
        { //spawn coin
            Instantiate(AssetCall.instance.coin, transform.position + (Vector3)Random.insideUnitCircle * .3f, Quaternion.identity);
        }
        else
        {
            if (Random.Range(0, 10) <= 3)
            { //spawn gem
                Instantiate(AssetCall.instance.gem, transform.position + (Vector3)Random.insideUnitCircle * .3f, Quaternion.identity);
            }
        }*/

        int coinLikelihood=0;
        int gemLikelihood=0;
        int oxygenLikelihood=0;
        int ammoLikelihood=0;
        int bombLikelihood=0;
        int shieldLikelihood=0;

        switch (GameManager.Instance.currentdataObj.enemyDropLevel)
        {
            case 0:
                coinLikelihood = 42;
                gemLikelihood = 3;
                break;
            case 1:
                coinLikelihood = 35;
                gemLikelihood = 3;
                oxygenLikelihood = 15;
                break;
            case 2:
                coinLikelihood = 35;
                gemLikelihood = 5;
                oxygenLikelihood = 17;
                bombLikelihood = 5;
                break;
            case 3:
                coinLikelihood = 30;
                gemLikelihood = 5;
                oxygenLikelihood = 20;
                bombLikelihood = 10;
                ammoLikelihood = 5;
                break;
            case 4:
                coinLikelihood = 30;
                gemLikelihood = 8;
                oxygenLikelihood = 20;
                bombLikelihood = 10;
                ammoLikelihood = 10;
                shieldLikelihood = 5;
                break;
            case 5:
                coinLikelihood = 25;
                gemLikelihood = 8;
                oxygenLikelihood = 20;
                bombLikelihood = 15;
                ammoLikelihood = 15;
                shieldLikelihood = 15;
                break;
        }


        int RandomVal = Random.Range(0, 101);
        int valueCheck = coinLikelihood;

        Debug.Log("RandomVal = " + RandomVal + "; Case = lvl " + GameManager.Instance.currentdataObj.enemyDropLevel);

        if (RandomVal <= valueCheck)
        {
            Instantiate(AssetCall.instance.coin, transform.position + (Vector3)Random.insideUnitCircle * .3f, Quaternion.identity);
            return;
        }

        Debug.Log(RandomVal + " > " + valueCheck);

        valueCheck += gemLikelihood;

        if (RandomVal <= valueCheck)
        {
            Instantiate(AssetCall.instance.gem, transform.position + (Vector3)Random.insideUnitCircle * .3f, Quaternion.identity);
            return;
        }

        Debug.Log(RandomVal + " > " + valueCheck);

        if (oxygenLikelihood <= 0) { return; }

        valueCheck += oxygenLikelihood;

        if (RandomVal <= valueCheck)
        {
            Instantiate(AssetCall.instance.oxygen, transform.position + (Vector3)Random.insideUnitCircle * .3f, Quaternion.identity);
            return;
        }

        Debug.Log("Failed Oxygen: " + RandomVal + " > " + valueCheck);

        if (bombLikelihood <= 0) { return; }

        valueCheck += bombLikelihood;

        if (RandomVal <= valueCheck)
        {
            Instantiate(GameManager.Instance.currentdataObj.hasBombs ? AssetCall.instance.bomb : AssetCall.instance.oxygen, transform.position + (Vector3)Random.insideUnitCircle * .3f, Quaternion.identity);
            return;
        }

        Debug.Log("Failed Bomb: " + RandomVal + " > " + valueCheck);

        if (ammoLikelihood <= 0) { return; }

        valueCheck += ammoLikelihood;

        if (RandomVal <= valueCheck)
        {
            Instantiate(GameManager.Instance.currentdataObj.hasGun ? AssetCall.instance.ammo : AssetCall.instance.oxygen, transform.position + (Vector3)Random.insideUnitCircle * .3f, Quaternion.identity);
            return;
        }

        Debug.Log("Failed Gun: " + RandomVal + " > " + valueCheck);

        if (shieldLikelihood <= 0) { return; }

        valueCheck += shieldLikelihood;

        if (RandomVal <= valueCheck)
        {
            Instantiate(GameManager.Instance.currentdataObj.hasShield ? AssetCall.instance.shield : AssetCall.instance.oxygen, transform.position + (Vector3)Random.insideUnitCircle * .3f, Quaternion.identity);
            return;
        }

        Debug.Log("Failed Shield: " + RandomVal + " > " + valueCheck);
    }

}
