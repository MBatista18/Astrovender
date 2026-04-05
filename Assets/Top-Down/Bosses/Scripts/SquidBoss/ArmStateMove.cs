using UnityEngine;
using System.Collections;

public class ArmStateMove : StateBase
{
    ArmManager armManager;

    public ArmStateMove(StateMachineBase sm) : base(sm)
    {
        armManager = (ArmManager)sm;
        //movingTowardsJab = Random.Range(0, 10) < 5 ? true : false;
        movingTowardsJab = armManager.startOnJab;
    }

    bool movingTowardsJab;

    float movementSpeed = 2f;

    Coroutine coroutine;

    public override void thisStart()
    {
        base.thisStart();

        if (movingTowardsJab)
        {
            Debug.Log("Move to jab");
            coroutine = armManager.StartCoroutine(MoveTowardJab());
        }
        else
        {
            Debug.Log("Move to swipe");
            coroutine = armManager.StartCoroutine(MoveTowardSwipe());
        }

        movingTowardsJab = !movingTowardsJab;
    }

    IEnumerator MoveTowardSwipe()
    {
        while (armManager.transform.position.y < armManager.startingPosition.y)
        {
            armManager.transform.position += Vector3.up * movementSpeed * Time.deltaTime;
            yield return null;
        }

        armManager.transform.position = new Vector3(armManager.transform.position.x, armManager.startingPosition.y, armManager.transform.position.z);

        float transformX = Random.Range(armManager.startingPosition.x + Mathf.Sign(armManager.xDistance) * 1, armManager.startingPosition.x + armManager.xDistance);

        while (armManager.armDirection < 0 ? armManager.transform.position.x > transformX : armManager.transform.position.x < transformX)
        {
            Debug.Log(transformX + " < " + armManager.transform.position.x);

            // while arm is facing right, wait until it reaches right
            // while arm is facing left, wait until it reaches left

            armManager.transform.position = new Vector3(armManager.transform.position.x + movementSpeed * Time.deltaTime * armManager.armDirection, 
                armManager.startingPosition.y, armManager.transform.position.z);
            yield return null;
        }

        armManager.transform.position = new Vector3(transformX, armManager.startingPosition.y, armManager.transform.position.z);

        armManager.ChangeState(armManager.stateSwipe);
    }

    IEnumerator MoveTowardJab()
    {
        Debug.Log("jab_move along X");

        while (armManager.armDirection < 0 ? armManager.transform.position.x < armManager.startingPosition.x : armManager.transform.position.x > armManager.startingPosition.x)
        {
            armManager.transform.position += Vector3.right * -armManager.armDirection * movementSpeed * Time.deltaTime;
            yield return null;
        }

        armManager.transform.position = new Vector3(armManager.startingPosition.x, armManager.transform.position.y, armManager.transform.position.z);

        Debug.Log("jab_move along Y");

        float transformY = Random.Range(armManager.startingPosition.y - 1, armManager.startingPosition.y + -armManager.yDistance);

        while (armManager.transform.position.y > transformY)
        {
            // while arm is facing right, wait until it reaches right
            // while arm is facing left, wait until it reaches left

            armManager.transform.position = new Vector3(armManager.transform.position.x,
                armManager.transform.position.y - movementSpeed * Time.deltaTime, armManager.transform.position.z);
            yield return null;
        }

        armManager.transform.position = new Vector3(armManager.startingPosition.x, transformY, armManager.transform.position.z);

        armManager.ChangeState(armManager.stateJab);
    }

    public override void thisEnd()
    {
        base.thisEnd();

        if (coroutine != null)
        {
            armManager.StopCoroutine(coroutine);
            coroutine = null;
        }
    }
}
