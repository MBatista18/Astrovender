using UnityEngine;

public class PelletRobotStateAttack : StateBase
{
    PelletRobotSM sm;

    public PelletRobotStateAttack (StateMachineBase _sm) : base(_sm)
    {
        sm = (PelletRobotSM)_sm;
    }

    float timer;
    float timerStartingVal = 2f;

    bool doOnce;
    public override void thisStart()
    {
        base.thisStart();
        timer = timerStartingVal;

        doOnce = false;
        sm.GetAnimator().Play("PelletShoot");
    }

    public override void thisUpdate()
    {
        base.thisUpdate();
        timer -= Time.deltaTime;

        if (timer <= timerStartingVal / 2 && !doOnce)
        {
            doOnce = true;

            sm.GetAudioCall().CallAudioClip("Shoot");

            var a = Object.Instantiate(sm.GetBullet(), sm.transform.position, Quaternion.identity);
            Vector3 playerPosRef = AssetCall.instance.playerSM.transform.position + ((Vector3)Random.insideUnitCircle * .6f);
            a.GetComponent<EnemyBullet>()?.SetDirection((playerPosRef - sm.transform.position));
        }

        if (timer <= 0)
        {
            sm.ChangeState(sm.AttackState());
        }
    }
}
