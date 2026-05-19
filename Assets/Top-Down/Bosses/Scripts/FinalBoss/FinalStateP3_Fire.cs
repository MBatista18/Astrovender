using UnityEngine;
using System.Collections;

public class FinalStateP3_Fire : StateBase
{
    FinalBossSM sm;

    public FinalStateP3_Fire(StateMachineBase _sm) : base(_sm)
    {
        sm = (FinalBossSM)_sm;
    }

    Coroutine a;

    public override void thisStart()
    {
        base.thisStart();
        a = sm.StartCoroutine(fire());
    }

    public override void thisEnd()
    {
        base.thisEnd();
        if (a != null)
        {
            sm.StopCoroutine(a);
            a = null;
        }
    }

    IEnumerator fire()
    {
        sm.GetAudioCall().CallAudioClip("PrepareAttack");

        yield return new WaitForSeconds(2f);

        sm.GetAudioCall().CallAudioClip("Shoot2");

        Object.Instantiate(sm.GetProjectile(), sm.transform.position, Quaternion.identity).
            GetComponent<EnemyBullet>()?.SetDirection(Vector2.up);
        Object.Instantiate(sm.GetProjectile(), sm.transform.position, Quaternion.identity).
            GetComponent<EnemyBullet>()?.SetDirection(new Vector2(1,1));
        Object.Instantiate(sm.GetProjectile(), sm.transform.position, Quaternion.identity).
            GetComponent<EnemyBullet>()?.SetDirection(Vector2.right);
        Object.Instantiate(sm.GetProjectile(), sm.transform.position, Quaternion.identity).
            GetComponent<EnemyBullet>()?.SetDirection(new Vector2(1, -1));
        Object.Instantiate(sm.GetProjectile(), sm.transform.position, Quaternion.identity).
            GetComponent<EnemyBullet>()?.SetDirection(Vector2.down);
        Object.Instantiate(sm.GetProjectile(), sm.transform.position, Quaternion.identity).
            GetComponent<EnemyBullet>()?.SetDirection(new Vector2(-1, -1));
        Object.Instantiate(sm.GetProjectile(), sm.transform.position, Quaternion.identity).
            GetComponent<EnemyBullet>()?.SetDirection(Vector2.left);
        Object.Instantiate(sm.GetProjectile(), sm.transform.position, Quaternion.identity).
            GetComponent<EnemyBullet>()?.SetDirection(new Vector2(-1, 1));

        yield return new WaitForSeconds(4f);

        sm.ChangeState(sm.stateP3_Move);
    }
}
