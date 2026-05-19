using UnityEngine;

public class FinalBoss2SM : EnemySM
{
    public FinalBoss2StateRun stateRun;
    public override StateBase InitialState()
    {
        return stateRun;
    }

    public FinalBoss2StateShoot stateShoot;

    public GameObject projectile;

    public override void InstantiateStates()
    {
        base.InstantiateStates();

        stateRun = new FinalBoss2StateRun(this);
        stateShoot = new FinalBoss2StateShoot(this);
    }
}
