using UnityEngine;
using System.Collections;

public class GunBossStateIdle : StateBase
{
    GunBossSM sm;
    bool bossOpening;

    public GunBossStateIdle (StateMachineBase _sm) : base(_sm)
    {
        sm = (GunBossSM)_sm;

        bossOpening = true;
    }

    Coroutine shootCoroutine;

    float timer;

    public override void thisStart()
    {
        base.thisStart();

        timer = Random.Range(.75f, 1.8f);

        if (bossOpening) { return; }

        switch (sm.GetFacingDirection())
        {
            case AstrovenderStructs.facingDirection.up:
                sm.GetAnimator().Play("Torso_IdleUp");
                break;
            case AstrovenderStructs.facingDirection.left:
                sm.GetAnimator().Play("Torso_IdleLeft");
                break;
            case AstrovenderStructs.facingDirection.right:
                sm.GetAnimator().Play("Torso_IdleRight");
                break;
            case AstrovenderStructs.facingDirection.down:
                sm.GetAnimator().Play("Torso_IdleDown");
                break;
        }

        shootCoroutine = sm.StartCoroutine(sm.shoot());
    }

    public override void thisUpdate()
    {
        base.thisUpdate();
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            sm.ChangeState(sm.AttackState());
        }
    }

    public override void thisEnd()
    {
        base.thisEnd();

        if (shootCoroutine != null)
        {
            sm.StopCoroutine(shootCoroutine);
            shootCoroutine = null;
        }

        bossOpening = false;
    }
}
