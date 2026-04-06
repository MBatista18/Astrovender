using UnityEngine;

public class BombBossSM : BossBaseSM
{
    [field: SerializeField] public Transform Player { get; private set; }
    [field: SerializeField] public GameObject EnergyBall { get; private set; }
    [field: SerializeField] public PortManager PortManager { get; private set; }

    [SerializeField] GameObject[] DungeonWalls;

    public override void OnEnableFunctions()
    {
        base.OnEnableFunctions();

        foreach (GameObject a in DungeonWalls)
        {
            a.SetActive(true);
        }

        PortManager.gameObject.SetActive(true);
        
        foreach (Port p in PortManager.PortsArray)
        {
            p.enabled = true;
        }
    }

    public override void OnDisableFunctions()
    {
        base.OnDisableFunctions();
        foreach (GameObject a in DungeonWalls)
        {
            a.SetActive(false);
        }

        PortManager.gameObject.SetActive(false);

        foreach (Port p in PortManager.PortsArray)
        {
            p.enabled = false;
        }
    }

    Animator animator;
    public Animator GetAnimator() => animator;

    AudioCall audioCall;
    public AudioCall GetAudioCall() { return audioCall; }
    public override void InstantiateComponents()
    {
        base.InstantiateComponents();

        audioCall = GetComponent<AudioCall>();
        animator = GetComponent<Animator>();
    }

    BombBossStateHidden stateHidden;
    public override StateBase InitialState()
    {
        return stateHidden;
    }
    public BombBossStateHidden GetStateHidden() { return stateHidden; }

    BombBossStateVisible stateVisible;
    public override StateBase AttackState()
    {
        return stateVisible;
    }

    BombBossStateVulnerable stateVulnerable;
    public BombBossStateVulnerable GetVulnerableState() => stateVulnerable;

    public override void InstantiateStates()
    {
        base.InstantiateStates();

        stateHidden = new BombBossStateHidden(this);
        stateVisible = new BombBossStateVisible(this);
        stateVulnerable = new BombBossStateVulnerable(this);
    }

    private Collider2D[] colliders;
    private SpriteRenderer spriteRenderer;
    private Canvas bossValues;
    public void SetVisibility(bool value)
    {
        foreach (Collider2D c in colliders)
        {
            c.enabled = value;
        }
        spriteRenderer.enabled = value;
        bossValues.enabled = value;
    }

    public override void InstantiateValues()
    {
        base.InstantiateValues();

        if (Player == null)
        {
            Player = GameObject.FindFirstObjectByType<PlayerStateMachine>().transform;
        }
        colliders = GetComponentsInChildren<Collider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        bossValues = GetComponentInChildren<Canvas>();
    }

    public override void UpdateFunctions()
    {
        base.UpdateFunctions();
    }
}
