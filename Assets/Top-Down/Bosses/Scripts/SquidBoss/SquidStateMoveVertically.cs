using UnityEngine;

public class SquidStateMoveVertically : StateBase
{
    SquidBossSM sm;

    public SquidStateMoveVertically(StateMachineBase _sm) : base(_sm)
    {
        sm = (SquidBossSM)_sm;

        moveVertically = Random.Range(0, 10) < 5 ? true : false;
    }

    bool moveVertically;

    float movementTimer;
    public override void thisStart()
    {
        base.thisStart();
        movementTimer = Random.Range(1f, 3f);
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        movementTimer -= Time.deltaTime;

        if (movementTimer <= 0) { sm.ChangeState(sm.InitialState()); return; }

        Vector3 pos = sm.transform.position;

        sm.transform.position = new Vector3(
            pos.x, Mathf.Clamp(pos.y + (sm.GetMovementSpeed() * (moveVertically ? 1 : -1) * Time.deltaTime), sm.GetMinimumY(), sm.GetMaximumY()), pos.z);
    }

    public override void thisEnd()
    {
        base.thisEnd();
        moveVertically = !moveVertically;
    }
}
