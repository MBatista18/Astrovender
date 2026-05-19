using UnityEngine;

public class FinalBossSM : BossBaseSM
{
    [SerializeField] GameObject laserRef;
    public GameObject GetLaser() { return laserRef; }

    [SerializeField] GameObject projectile;
    public GameObject GetProjectile() { return projectile; }

    [SerializeField] GameObject bossPart2;

    public FinalStateP1_Pause stateP1_Pause;

    public override StateBase InitialState()
    {
        return stateP1_Pause;
    }

    public FinalStateP1_Run stateP1_Run;
    public FinalStateP2_Pause stateP2_Pause;
    public FinalStateP2_Rocket stateP2_Rocket;
    public FinalStateP3_Fire stateP3_Fire;
    public FinalStateP3_Move stateP3_Move;

    public override StateBase DeathState()
    {
        Instantiate(bossPart2, transform.position, Quaternion.identity);
        return base.DeathState();
    }

    public override void InstantiateStates()
    {
        base.InstantiateStates();

        stateP1_Pause = new FinalStateP1_Pause(this);
        stateP1_Run = new FinalStateP1_Run(this);
        stateP2_Pause = new FinalStateP2_Pause(this);
        stateP2_Rocket = new FinalStateP2_Rocket(this);
        stateP3_Fire = new FinalStateP3_Fire(this);
        stateP3_Move = new FinalStateP3_Move(this);
    }

    BoxCollider2D boxCollider;
    public BoxCollider2D GetCollider2D() { return boxCollider; }
    BoxCollider2D childCollider;

    AudioCall audioCall;
    public AudioCall GetAudioCall() { return audioCall; }

    public override void InstantiateComponents()
    {
        base.InstantiateComponents();
        boxCollider = GetComponent<BoxCollider2D>();
        childCollider = transform.Find("PlayerCollider").GetComponent<BoxCollider2D>();
        audioCall = GetComponent<AudioCall>();
    }

    public GameObject particleTransition;

    public override void TakeDamage(int damageAmount, Vector3 attackerPos)
    {
        base.TakeDamage(damageAmount, attackerPos);


        switch (phaseIndex)
        {
            case 1:
                if (((float)GetHealth() / (float)GetMaxHealth()) < .66f)
                {
                    SetPhase();

                    Debug.Log("next phase");
                }
                break;
            case 2:
                if (((float)GetHealth() / (float)GetMaxHealth()) < .33f)
                {
                    SetPhase();

                    Debug.Log("next phase");
                }
                break;
        }
    }

    int phaseIndex = 1;

    public void SetPhase()
    {
        phaseIndex++;

        GetAudioCall().CallAudioClip("Transition");

        Instantiate(particleTransition, transform);

        switch (phaseIndex)
        {
            case 1:
                ChangeState(stateP1_Pause);
                boxCollider.size = new Vector3(2.25f, 4f);
                return;
            case 2:
                ChangeState(stateP2_Pause);
                boxCollider.size = Vector2.one * 2f;
                childCollider.size = Vector2.one * 1.8f;
                return;
            case 3:
                ChangeState(stateP3_Move);
                boxCollider.size = Vector2.one * 1.5f;
                childCollider.size = Vector2.one * 1.2f;
                return;
        }
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
