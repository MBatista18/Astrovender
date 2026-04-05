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
    public override void thisStart()
    {
        base.thisStart();

        sm.SetVisibility(false);
        if (portManager.GetRandomActivePort(out Port port))
        {
            port.HideInPort();
        }
        hideTimer = Random.Range(4.25f, 5f);

        Debug.Log("Hidden");
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        hideTimer -= Time.deltaTime;

        if (hideTimer <= 0)
        {
            sm.ChangeState(sm.AttackState()); // Pop up and start attacking
        }
    }

    public override void thisEnd()
    {
        base.thisEnd();
        sm.SetVisibility(true);
    }
}
