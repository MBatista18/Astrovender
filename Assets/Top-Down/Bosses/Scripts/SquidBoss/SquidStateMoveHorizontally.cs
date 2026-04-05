using UnityEngine;

public class SquidStateMoveHorizontally : StateBase
{
    SquidBossSM sm;

    public SquidStateMoveHorizontally(StateMachineBase _sm) : base(_sm)
    {
        sm = (SquidBossSM)_sm;

        moveRight = Random.Range(0, 10) < 5 ? true : false;
    }

    bool moveRight;

    float movementTimer;
    public override void thisStart()
    {
        base.thisStart();
        movementTimer = Random.Range(1f, 3f);
        Debug.Log("Horizontal");
        sm.GetAnimator().Play(!moveRight ? "SquidMoveLeft" : "SquidMoveRight");

        sm.GetAudioCall().CallAudioClip("MonsterCry2");
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        movementTimer -= Time.deltaTime;

        if (movementTimer <= 0) { sm.ChangeState(sm.InitialState()); return; }

        Vector3 pos = sm.transform.position;

        sm.transform.position = new Vector3(Mathf.Clamp(pos.x + (sm.GetMovementSpeed() * (moveRight ? 1 : -1) * Time.deltaTime), sm.GetMinimumX(), sm.GetMaximumX()), 
            pos.y, pos.z);
    }

    public override void thisEnd()
    {
        base.thisEnd();
        moveRight = !moveRight;
    }
}
