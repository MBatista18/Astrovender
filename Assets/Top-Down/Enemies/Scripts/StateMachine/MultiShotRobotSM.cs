using UnityEngine;

public class MultiShotRobotSM : EnemySM
{
    [SerializeField] GameObject bullet;
    public GameObject GetBullet() { return bullet; }

    MultiShotRobotStateFire stateFire;
    public override StateBase InitialState()
    {
        return stateFire;
    }
    public override void InstantiateStates()
    {
        stateFire = new MultiShotRobotStateFire(this);
    }

    public override void InstantiateValues()
    {
        base.InstantiateValues();
        base.SetReactToDamage(false);
    }
}
