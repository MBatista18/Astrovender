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
}
