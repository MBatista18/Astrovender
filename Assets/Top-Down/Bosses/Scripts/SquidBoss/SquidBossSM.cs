using UnityEngine;

public class SquidBossSM : BossBaseSM
{
    SquidStateIdle stateIdle;
    public override StateBase InitialState()
    {
        return stateIdle;
    }
    SquidStateMoveHorizontally stateMoveHorizontally;
    public SquidStateMoveHorizontally GetStateHorizontal() { return stateMoveHorizontally; }
    SquidStateMoveVertically stateMoveVertically;
    public SquidStateMoveVertically GetStateVertical() { return stateMoveVertically; }

    public override void InstantiateStates()
    {
        base.InstantiateStates();

        stateIdle = new SquidStateIdle(this);
        stateMoveHorizontally = new SquidStateMoveHorizontally(this);
        stateMoveVertically = new SquidStateMoveVertically(this);
    }

    float minimumX;
    public float GetMinimumX() { return minimumX; }
    float maximumX;
    public float GetMaximumX() { return maximumX; }

    float minimumY;
    public float GetMinimumY() { return minimumY; }
    float maximumY;
    public float GetMaximumY() { return maximumY; }

    [SerializeField] float minimumTravelDistanceX;
    [SerializeField] float maximumTravelDistanceX;

    [SerializeField] float maximumTravelDistanceY;

    public override void InstantiateValues()
    {
        base.InstantiateValues();

        minimumX = transform.position.x + minimumTravelDistanceX;
        maximumX = transform.position.x + maximumTravelDistanceX;

        minimumY = transform.position.y;
        maximumY = transform.position.y + maximumTravelDistanceY;
    }

    [Header("Walls")]
    [SerializeField] GameObject[] DungeonWalls;

    [Header("Arms")]

    [SerializeField] GameObject leftArm;
    [SerializeField] GameObject rightArm;

    public override void OnEnableFunctions()
    {
        base.OnEnableFunctions();

        foreach (GameObject a in DungeonWalls)
        {
            a.SetActive(true);
        }

        leftArm.SetActive(true);
        rightArm.SetActive(true);
    }

    public override void OnDisableFunctions()
    {
        base.OnDisableFunctions();

        foreach (GameObject a in DungeonWalls)
        {
            a.SetActive(false);
        }

        leftArm.SetActive(false);
        rightArm.SetActive(false);
    }

    Animator animator;
    public Animator GetAnimator()
    {
        return animator;
    }

    AudioCall audioCall;
    public AudioCall GetAudioCall() { return audioCall; }

    public override void InstantiateComponents()
    {
        base.InstantiateComponents();
        animator = GetComponent<Animator>();
        audioCall = GetComponent<AudioCall>();
    }
}
