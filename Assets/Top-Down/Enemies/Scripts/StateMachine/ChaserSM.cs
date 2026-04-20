using UnityEngine;

public class ChaserSM : EnemySM
{
    ChaserStateChase stateChase;

    public override void InstantiateStates()
    {
        base.InstantiateStates();
        stateChase = new ChaserStateChase(this);
    }
    public override StateBase AttackState()
    {
        return stateChase;
    }

    AudioSource audioSource;
    public AudioSource GetAudioSource()
    {
        return audioSource;
    }

    public override void OnShieldReaction()
    {
        GetStateKnockback().SetKnockback((Vector2)(transform.position - AssetCall.instance.playerSM.transform.position), 1f);
        ChangeState(GetStateKnockback());
    }

    public override void InstantiateComponents()
    {
        base.InstantiateComponents();
        audioSource = GetComponent<AudioSource>();
    }
}