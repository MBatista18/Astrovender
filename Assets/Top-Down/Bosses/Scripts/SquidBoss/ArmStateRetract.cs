using UnityEngine;

using System.Collections;

public class ArmStateRetract : StateBase
{
    ArmManager armManager;

    public ArmStateRetract(StateMachineBase sm) : base(sm)
    {
        armManager = (ArmManager)sm;
    }

    float armRetractSpeed = 4f;

    Coroutine currentCoroutine;

    public override void thisStart()
    {
        base.thisStart();
        Debug.Log("retract");

        currentCoroutine = armManager.StartCoroutine(StretchOutArm());
    }


    IEnumerator StretchOutArm()
    {
        for (int i = armManager.armSegments.Length - 1; i > 0; i--)
        {
            if (armManager.armSegments[i].GetIsWeakPoint()) { armManager.armSegments[i].CanRetractNow = false; }

            while (Vector3.Distance(armManager.armSegments[i].transform.localPosition, Vector3.zero) > .001f)
            {
                armManager.armSegments[i].transform.localPosition =
                    Vector3.MoveTowards(armManager.armSegments[i].transform.localPosition, Vector3.zero, 
                    armRetractSpeed * Time.deltaTime);

                yield return null;
            }

            armManager.armSegments[i].transform.localPosition = Vector3.zero;
        }

        for (int i = 0; i < armManager.armSegments.Length; i++)
        {
            armManager.armSegments[i].transform.rotation = Quaternion.Euler(Vector3.zero);

            yield return null;
        }


        yield return new WaitForSeconds(1f);

        armManager.ChangeState(armManager.stateMove);
    }

    public override void thisEnd()
    {
        base.thisEnd();

        if (currentCoroutine != null)
        {
            armManager.StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
    }
}
