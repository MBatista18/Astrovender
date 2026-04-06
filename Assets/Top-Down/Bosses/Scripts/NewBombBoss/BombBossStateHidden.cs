using UnityEngine;

public class BombBossStateHidden : StateBase
{
    // The boss will be temporarily hidden and invulnerable
    // After a short time, it will pop up through a random active port and start attacking
    BombBossSM sm;
    PortManager portManager;

    public BombBossStateHidden(StateMachineBase _sm) : base(_sm)
    {
        sm = (BombBossSM)_sm;
        portManager = sm.PortManager;
    }

    float hideTimer;
    float maxHideTimer;

    Port referencePort;

    public override void thisStart()
    {
        base.thisStart();

        sm.SetVisibility(false);
        if (portManager.GetRandomActivePort(out Port port))
        {
            referencePort = port;
            port.HideInPort();
        }
        maxHideTimer = Random.Range(4.25f, 5f);
        hideTimer = maxHideTimer;

        Debug.Log("Hidden");
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        hideTimer -= Time.deltaTime;

        if (hideTimer <= maxHideTimer * .15f)
        {
            referencePort?.OpenPort();
        }

        if (hideTimer <= 0)
        {
            sm.ChangeState(sm.AttackState()); // Pop up and start attacking
        }
    }

    public override void thisEnd()
    {
        base.thisEnd();
        referencePort = null;
        sm.SetVisibility(true);
    }
}
