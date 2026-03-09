using UnityEngine;

public class BombRobotStatePatrol : EnemyStatePatrol
{
    BombRobotSM sm;

    public BombRobotStatePatrol(StateMachineBase _sm) : base(_sm)
    {
        sm = (BombRobotSM)_sm;
    }

    Vector3 lastPos;
    bool isFacingDown;

    public override void thisStart()
    {
        base.thisStart();

        lastPos = sm.transform.position;
    }

    public override void thisUpdate()
    {
        base.thisUpdate();


        if (sm.transform.position.x > lastPos.x)
        {
            //Debug.Log("moving right, " + sm.transform.position.x + " > " + lastPos.x);
            sm.transform.localScale = new Vector3(-1, 1, 1);
        }

        if (sm.transform.position.x < lastPos.x)
        {
            //Debug.Log("moving left, " + sm.transform.position.x + " < " + lastPos.x);
            sm.transform.localScale = new Vector3(1, 1, 1);
        }

        if (sm.transform.position.y > lastPos.y)
        {
            if (isFacingDown) 
            {
                isFacingDown = false;

                sm.GetAnimator().Play("BombFacingUp");
            }
        }

        if (sm.transform.position.y < lastPos.y)
        {
            if (!isFacingDown)
            {
                isFacingDown = true;

                sm.GetAnimator().Play("BombFacingDown"); 
            }
        }

        lastPos = sm.transform.position;
    }
}
