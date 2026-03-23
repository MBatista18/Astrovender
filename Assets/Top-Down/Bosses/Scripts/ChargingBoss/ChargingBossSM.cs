using UnityEngine;

public class ChargingBossSM : BossBaseSM
{
    ChargingBossStateBurrow stateBurrow;
    public ChargingBossStateBurrow GetStateBurrow() { return stateBurrow; }
    ChargingBossStateBurrowAlign stateBurrowAlign;
    public ChargingBossStateBurrowAlign GetStateBurrowAlign() { return stateBurrowAlign; }
    ChargingBossStateCharge stateCharge;
    public ChargingBossStateCharge GetStateCharge() { return stateCharge; }
    ChargingBossStateStunned stateStunned;
    public ChargingBossStateStunned GetStateStunned() { return stateStunned; }

    ChargingBossStateInitial stateInitial;
    public override StateBase InitialState()
    {
        return stateInitial;
    }

    public override void InstantiateStates()
    {
        base.InstantiateStates();
        stateBurrow = new ChargingBossStateBurrow(this);
        stateBurrowAlign = new ChargingBossStateBurrowAlign(this);
        stateCharge = new ChargingBossStateCharge(this);
        stateInitial = new ChargingBossStateInitial(this);
        stateStunned = new ChargingBossStateStunned(this);
    }

    [Header("Boss Values")]
    [SerializeField] float burrowingTime;
    public float GetBurrowingTime() { return burrowingTime; }
    [SerializeField] float burrowingSpeed;
    public float GetBurrowingSpeed() { return burrowingSpeed; }
    [SerializeField] float stuntime;
    public float GetStunTime() { return stuntime; }

    float bottomY;
    public float GetBottomY() { return bottomY; }
    float topY;
    public float GetTopY() { return topY; }
    float leftX;
    public float GetLeftX() { return leftX; }
    float rightX;
    public float GetRightX() { return rightX; }

    float colliderSizeBuffer;

    public override void InstantiateValues()
    {
        base.InstantiateValues();

        colliderSizeBuffer = GetComponent<BoxCollider2D>().size.x / 2;

        bottomY = Physics2D.Raycast(transform.position, Vector2.down, 20f, LayerMask.GetMask("Walls")).point.y + colliderSizeBuffer;
        topY = Physics2D.Raycast(transform.position, Vector2.up, 20f, LayerMask.GetMask("Walls")).point.y - colliderSizeBuffer;

        leftX = Physics2D.Raycast(transform.position, Vector2.left, 20f, LayerMask.GetMask("Walls")).point.x + colliderSizeBuffer;
        rightX = Physics2D.Raycast(transform.position, Vector2.right, 20f, LayerMask.GetMask("Walls")).point.x - colliderSizeBuffer;

        Debug.Log(Physics2D.Raycast(transform.position, Vector2.left, 20f, LayerMask.GetMask("Walls")).collider.gameObject.name);

        Debug.Log("Left = " + leftX + "; Right = " + rightX);
    }

    Animator animator;
    public Animator GetAnimator() { return animator; }

    BoxCollider2D boxCollider2D;
    public BoxCollider2D GetBoxCollider2D() { return boxCollider2D; }

    SpriteRenderer sp_renderer;
    public SpriteRenderer GetSpriteRenderer() { return sp_renderer; }

    [SerializeField] FallingRockManager rockManager;
    public FallingRockManager GetRockSlideManager() { return rockManager; }


    public override void InstantiateComponents()
    {
        base.InstantiateComponents();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        sp_renderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
    }

    [SerializeField] GameObject[] DungeonWalls;

    public override void OnEnableFunctions()
    {
        base.OnEnableFunctions();

        foreach (GameObject a in DungeonWalls)
        {
            a.SetActive(true);
        }
    }

    public override void OnDisableFunctions()
    {
        base.OnDisableFunctions();
        foreach (GameObject a in DungeonWalls)
        {
            a.SetActive(false);
        }
    }
}
