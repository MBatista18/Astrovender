using UnityEngine;

public class BombBossSM : BossBaseSM
{
    [SerializeField] PortManager portManager;
    public PortManager GetPorts() { return portManager; }

    [SerializeField] GameObject[] DungeonWalls;

    public override void OnEnableFunctions()
    {
        base.OnEnableFunctions();

        foreach (GameObject a in DungeonWalls)
        {
            a.SetActive(true);
        }

        portManager.gameObject.SetActive(true);
    }

    public override void OnDisableFunctions()
    {
        base.OnDisableFunctions();
        foreach (GameObject a in DungeonWalls)
        {
            a.SetActive(false);
        }
        
        portManager.gameObject.SetActive(false);
    }

    Animator animator;
    public Animator GetAnimator() { return animator; }

    public override void InstantiateComponents()
    {
        base.InstantiateComponents();

        animator = GetComponent<Animator>();
    }

    BombBossStateHidden stateHidden;
    public override StateBase InitialState()
    {
        return stateHidden;
    }

    BombBossStateVisible stateVisible;
    public override StateBase AttackState()
    {
        return stateVisible;
    }

    BombBossStateVulnerable stateVulnerable;
    public BombBossStateVulnerable GetVulnerableState() { return stateVulnerable; }

    public override void InstantiateStates()
    {
        base.InstantiateStates();

        stateHidden = new BombBossStateHidden(this);
        stateVisible = new BombBossStateVisible(this);
        stateVulnerable = new BombBossStateVulnerable(this);
    }

    private Collider2D[] colliders;
    private SpriteRenderer spriteRenderer;
    public void SetVisibility(bool value)
    {
        foreach (Collider2D c in colliders)
        {
            c.enabled = value;
        }
        spriteRenderer.enabled = value;
    }

    Vector3 lastPosition;

    public override void InstantiateValues()
    {
        base.InstantiateValues();
        colliders = GetComponentsInChildren<Collider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
