using UnityEngine;

public class SquidStateMoveVertically : StateBase
{
    SquidBossSM sm;

    public SquidStateMoveVertically(StateMachineBase _sm) : base(_sm)
    {
        sm = (SquidBossSM)_sm;

        moveUp = true;
    }

    bool moveUp;

    float movementTimer;
    public override void thisStart()
    {
        base.thisStart();
        movementTimer = Random.Range(1f, 3f);
        Debug.Log("Vertical"); //
        sm.GetAnimator().Play("SquidMoveVertical");
        sm.GetAudioCall().CallAudioClip("MonsterCry1");
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        if (moveUp ? sm.transform.position.y >= sm.GetMaximumY() : sm.transform.position.y <= sm.GetMinimumY()) 
        {
            sm.ChangeState(sm.InitialState()); return;
        }
        Debug.Log("Ended");


        Vector3 pos = sm.transform.position;

        sm.transform.position = new Vector3(
            pos.x, Mathf.Clamp(pos.y + (sm.GetMovementSpeed() * 2 * (moveUp ? 1 : -1) * Time.deltaTime), sm.GetMinimumY(), sm.GetMaximumY()), pos.z);
    }

    public override void thisEnd()
    {
        base.thisEnd();
        moveUp = !moveUp;
    }
}
