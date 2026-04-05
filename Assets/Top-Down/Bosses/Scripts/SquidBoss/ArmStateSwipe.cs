using UnityEngine;
using System.Collections;

public class ArmStateSwipe : StateBase
{
    ArmManager armManager;

    public ArmStateSwipe (StateMachineBase sm) : base (sm)
    {
        armManager = (ArmManager)sm;
    }


    Coroutine currentCoroutine;

    public override void thisStart()
    {
        base.thisStart();
        Debug.Log("swipe");

        currentCoroutine = armManager.StartCoroutine(rotate());
    }

    float armSpeed = 3f;

    IEnumerator rotate()
    {

        armManager?.GetAudioCall().CallAudioClip("TentaclePeek");

        int finalIndex = armManager.armSegments.Length - 1;

        while (Mathf.Abs(armManager.armSegments[finalIndex].transform.localPosition.y) < 1) // check if this segment has moved more than a single unit to either side
        {
            Vector3 localPos = armManager.armSegments[finalIndex].transform.localPosition;

            localPos = new Vector3(localPos.x, localPos.y - (armSpeed * Time.deltaTime), localPos.z);

            armManager.armSegments[finalIndex].transform.localPosition = localPos;

            yield return null;
        }

        Vector3 lastSegment_localPos = armManager.armSegments[finalIndex].transform.localPosition;

        armManager.armSegments[finalIndex].transform.localPosition = new Vector3(lastSegment_localPos.x, -1, lastSegment_localPos.z);

        yield return new WaitForSeconds(1f);

        armManager?.GetAudioCall().CallAudioClip("TentacleExtend");

        float movementValuePerSegment = 0;

        while (movementValuePerSegment < 1)
        {
            for (int i = 1; i < finalIndex; i++)
            {
                if (armManager.armSegments[i].GetIsWeakPoint()) { armManager.armSegments[i].CanRetractNow = true; }

                movementValuePerSegment += armSpeed * Time.deltaTime;

                Vector3 localPos = armManager.armSegments[i].transform.localPosition;

                armManager.armSegments[i].transform.localPosition = new Vector3(localPos.x, -movementValuePerSegment, localPos.z);

                yield return null;
            }
        }

        for (int i = 1; i < finalIndex; i++)
        {
            Vector3 localPos = armManager.armSegments[i].transform.localPosition;
            armManager.armSegments[i].transform.localPosition = new Vector3(localPos.x, -1, localPos.z);
        }

        yield return new WaitForSeconds(.5f);

        float rotateSpeed = 90f;
        float fullSwingDuration = .4f;
       // float rotationFullZ = 30;

        Quaternion defaultRotation = Quaternion.Euler(Vector3.zero);
       // Quaternion targetRotation = Quaternion.Euler(new Vector3(0,0,armManager.armDirection * rotationFullZ));

        float timePerSwing = fullSwingDuration;

        armManager?.GetAudioCall().CallAudioClip("TentacleSwipe1");

        while (timePerSwing > 0)
        {
            timePerSwing -= Time.deltaTime;

            for (int i = 0; i < finalIndex; i++)
            {
                armManager.armSegments[i].transform.Rotate(new Vector3(0,0, rotateSpeed * armManager.armDirection * Time.deltaTime), Space.Self);

                yield return null;
            }
        }

        timePerSwing = fullSwingDuration;

        armManager?.GetAudioCall().CallAudioClip("TentacleSwipe2");

        while (timePerSwing > 0)
        {
            timePerSwing -= Time.deltaTime;

            for (int i = 0; i < finalIndex; i++)
            {
                armManager.armSegments[i].transform.Rotate(new Vector3(0, 0, rotateSpeed * -armManager.armDirection * Time.deltaTime), Space.Self);

                yield return null;
            }
        }

        timePerSwing = fullSwingDuration;

        armManager?.GetAudioCall().CallAudioClip("TentacleSwipe1");

        while (timePerSwing > 0)
        {
            timePerSwing -= Time.deltaTime;

            for (int i = 0; i < finalIndex; i++)
            {
                armManager.armSegments[i].transform.Rotate(new Vector3(0, 0, rotateSpeed * armManager.armDirection * Time.deltaTime), Space.Self);
                yield return null;
            }
        }


        yield return new WaitForSeconds(.3f);

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
