using UnityEngine;

public class ChaserStateChase : StateBase
{
    ChaserSM sm;

    public ChaserStateChase(StateMachineBase _sm) : base(_sm)
    {
        sm = (ChaserSM) _sm;
    }

    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();

        if (Vector2.Distance(sm.GetPlayerTransform().position, sm.transform.position) > sm.GetDetectionRadius() * 1.5f)
            // if the player leaves the detection radius (buffer of 1.5 added to detection radius to prevent player from just marginally leaving radius to attack again)
        {
            sm.ChangeState(sm.InitialState());
        }

        sm.GetRigidbody2D().MovePosition(Vector2.MoveTowards(sm.GetRigidbody2D().position, sm.GetPlayerTransform().position, sm.GetMovementSpeed() * Time.fixedDeltaTime));
    }
}
