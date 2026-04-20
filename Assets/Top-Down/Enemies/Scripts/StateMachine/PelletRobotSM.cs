using UnityEngine;

public class PelletRobotSM : EnemySM
{
    
    PelletRobotStateAttack stateAttack;
    public override StateBase InitialState()
    {
        return stateAttack;
    }

    PelletRobotStateMove stateMove;
    public override StateBase AttackState()
    {
        return stateMove;
    }

    public override void InstantiateStates()
    {
        base.InstantiateStates();

        stateAttack = new PelletRobotStateAttack(this);
        stateMove = new PelletRobotStateMove(this);
    }

    AudioCall audioCall;
    public AudioCall GetAudioCall() { return audioCall; }

    [SerializeField] GameObject bullet;
    public GameObject GetBullet() { return bullet; }

    public override void InstantiateComponents()
    {
        base.InstantiateComponents();
        audioCall = GetComponent<AudioCall>();
    }
}
