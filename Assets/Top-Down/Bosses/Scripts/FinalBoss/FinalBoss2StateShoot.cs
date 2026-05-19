using UnityEngine;

public class FinalBoss2StateShoot : StateBase
{
    FinalBoss2SM sm;

    AudioSource audioSource;

    public FinalBoss2StateShoot(StateMachineBase _sm) : base(_sm)
    {
        sm = (FinalBoss2SM)_sm;
        audioSource = sm.GetComponent<AudioSource>();
    }

    float timer;

    public override void thisStart()
    {
        base.thisStart();
        timer = .5f;

        audioSource.Play();

        var a = Object.Instantiate(sm.projectile, sm.transform.position, Quaternion.identity);
        a.GetComponent<EnemyBullet>().SetDirection(sm.stateRun.GetMovementDirection());
        sm.GetAnimator().Play("FB2_Idle");
    }

    public override void thisUpdate()
    {
        base.thisUpdate();
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            sm.ChangeState(sm.stateRun);
        }
    }
}
