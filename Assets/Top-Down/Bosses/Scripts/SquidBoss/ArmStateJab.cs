using UnityEngine;
using System.Collections;

public class ArmStateJab : StateBase
{
    ArmManager armManager;

    public ArmStateJab(StateMachineBase sm) : base(sm)
    {
        armManager = (ArmManager)sm;
    }

    float armSpeed = 3f;

    Coroutine currentCoroutine;

    public override void thisStart()
    {
        base.thisStart();

        Debug.Log("Jab");

        currentCoroutine = armManager.StartCoroutine(StretchOutArm());
    }

    IEnumerator StretchOutArm()
    {
        Debug.Log("Begin Stretch");

        armManager?.GetAudioCall().CallAudioClip("TentaclePeek");

        int finalIndex = armManager.armSegments.Length - 1;

        while (Mathf.Abs(armManager.armSegments[finalIndex].transform.localPosition.x) < 1) // check if this segment has moved more than a single unit to either side
        {
            Vector3 localPos = armManager.armSegments[finalIndex].transform.localPosition;

            localPos = new Vector3(localPos.x + (armSpeed * armManager.armDirection * Time.deltaTime), localPos.y, localPos.z);

            armManager.armSegments[finalIndex].transform.localPosition = localPos;

            yield return null;
        }

        Vector3 lastSegment_localPos = armManager.armSegments[finalIndex].transform.localPosition;

        armManager.armSegments[finalIndex].transform.localPosition = new Vector3(1 * armManager.armDirection, lastSegment_localPos.y, lastSegment_localPos.z);

        yield return new WaitForSeconds(1f);

        Debug.Log("Then Stretch");
        armManager?.GetAudioCall().CallAudioClip("TentacleExtend");

        for (int i = finalIndex - 1; i > 0; i--)
        {
            float movementValuePerSegment = 0;

            if (armManager.armSegments[i].GetIsWeakPoint()) { armManager.armSegments[i].CanRetractNow = true; }

            while (Mathf.Abs(movementValuePerSegment) < 1)
            {
                movementValuePerSegment += armSpeed * armManager.armDirection * Time.deltaTime;

                Vector3 localPos = armManager.armSegments[i].transform.localPosition;

                armManager.armSegments[i].transform.localPosition = new Vector3(movementValuePerSegment, localPos.y, localPos.z);

                yield return null;
            }
        }

        for (int i = 1; i < finalIndex; i++)
        {
            Vector3 localPos = armManager.armSegments[i].transform.localPosition;
            armManager.armSegments[i].transform.localPosition = new Vector3(1 * armManager.armDirection, localPos.y, localPos.z);
        }

        yield return new WaitForSeconds(3f);

        Debug.Log("End Stretch");

        armManager.ChangeState(armManager.stateRetract);
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
