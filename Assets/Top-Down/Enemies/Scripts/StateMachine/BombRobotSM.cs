using UnityEngine;

public class BombRobotSM : EnemySM
{
    [SerializeField] float fuseTime;
    public float GetFuseTime() { return fuseTime; }

    AudioCall audioCall;
    public AudioCall GetAudioCall()
    {
        return audioCall;
    }

    public override void InstantiateComponents()
    {
        base.InstantiateComponents();

        audioCall = GetComponent<AudioCall>();
    }

    public override void OnShieldReaction()
    {
        GetStateKnockback().SetKnockback((Vector2)(transform.position - AssetCall.instance.playerSM.transform.position), 1f);
        ChangeState(GetStateKnockback());
    }

    BombRobotStateAttack stateAttack;

    public override StateBase AttackState()
    {
        return stateAttack;
    }

    BombRobotStatePatrol statePatrol;
    public override StateBase InitialState()
    {
        return statePatrol;
    }

    public override void InstantiateStates()
    {
        base.InstantiateStates();

        stateAttack = new BombRobotStateAttack(this);
        statePatrol = new BombRobotStatePatrol(this);
    }
}
