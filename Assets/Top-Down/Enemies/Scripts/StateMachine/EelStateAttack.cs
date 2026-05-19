using System.Collections;
using UnityEngine;

public class EelStateAttack : StateBase
{
    private EelSM sm;

    private Coroutine attackCoroutine;

    private readonly Vector2[] shockOffsets =
    {
        new Vector2(-1, 1),
        new Vector2(0, 1),
        new Vector2(1, 1),

        new Vector2(-1, 0),
        new Vector2(1, 0),

        new Vector2(-1, -1),
        new Vector2(0, -1),
        new Vector2(1, -1),
    };

    public EelStateAttack(StateMachineBase _sm) : base(_sm)
    {
        sm = (EelSM)_sm;
    }

    public override void thisStart()
    {
        attackCoroutine = sm.StartCoroutine(AttackRoutine());

        sm.GetAudioCall().CallAudioClip("charge");

        sm.GetAnimator().Play("EelShock");
    }

    public override void thisEnd()
    {
        if (attackCoroutine != null)
        {
            sm.StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        sm.rb.linearVelocity = Vector2.zero;
    }

    private IEnumerator AttackRoutine()
    {
        //while (Vector2.Distance(sm.GetPlayerTransform().position, sm.transform.position) <= sm.GetDetectionRadius())
        //{
            sm.rb.linearVelocity = Vector2.zero;

            yield return new WaitForSeconds(sm.windupDuration);

            sm.GetAudioCall().CallAudioClip("shock");

            foreach (Vector2 offset in shockOffsets)
            {
                Vector3 spawnPos = sm.transform.position + (Vector3)(offset * sm.tileSize);
                Object.Instantiate(sm.shockZonePrefab, spawnPos, Quaternion.identity);
            }

            yield return new WaitForSeconds(sm.shockDuration);

            sm.rb.linearVelocity = Vector2.zero;

            yield return new WaitForSeconds(sm.postShockWaitDuration);

            /*while (
                Vector2.Distance(sm.GetPlayerTransform().position, sm.transform.position) > sm.shockRange &&
                Vector2.Distance(sm.GetPlayerTransform().position, sm.transform.position) <= sm.GetDetectionRadius()
            )
            {
                Vector2 chaseDir =
                    (sm.GetPlayerTransform().position - sm.transform.position).normalized;

                sm.rb.linearVelocity = chaseDir * sm.chaseSpeed;

                yield return null;
            }*/
        //}

        sm.rb.linearVelocity = Vector2.zero;
        sm.ChangeState(sm.statePatrol);
    }
}
