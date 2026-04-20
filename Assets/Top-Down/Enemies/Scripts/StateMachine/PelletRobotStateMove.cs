using UnityEngine;

public class PelletRobotStateMove : StateBase
{
    PelletRobotSM sm;

    public PelletRobotStateMove(StateMachineBase _sm) : base(_sm)
    {
        sm = (PelletRobotSM) _sm;
    }

    Vector2 movementDirection;
    float movementTimer;

    public override void thisStart()
    {
        base.thisStart();

        // finds a random direction to move in

        movementDirection = Random.insideUnitCircle.normalized;

        // re-orients movement direction in case it opts to move in the direction of a wall

        if ((movementDirection.y < 0 && Physics2D.Raycast(sm.transform.position, Vector2.down, 2f, LayerMask.GetMask("Walls"))) ||
            (movementDirection.y > 0 && Physics2D.Raycast(sm.transform.position, Vector2.up, 2f, LayerMask.GetMask("Walls"))))
        {
            movementDirection = new Vector2(movementDirection.x, -movementDirection.y);
        }

        if ((movementDirection.x < 0 && Physics2D.Raycast(sm.transform.position, Vector2.left, 2f, LayerMask.GetMask("Walls"))) ||
            (movementDirection.x > 0 && Physics2D.Raycast(sm.transform.position, Vector2.right, 2f, LayerMask.GetMask("Walls"))))
        {
            movementDirection = new Vector2(-movementDirection.x, movementDirection.y);
        }

        if (Mathf.Abs(movementDirection.y) > Mathf.Abs(movementDirection.x))
        {
            if (movementDirection.y > 0)
            {
                sm.GetAnimator().Play("PelletUp");
            }
            else
            {
                sm.GetAnimator().Play("PelletDown");
            }
        }
        else
        {
            if (movementDirection.x < 0)
            {
                sm.GetAnimator().Play("PelletLeft");
            }
            else
            {
                sm.GetAnimator().Play("PelletRight");
            }
        }
        movementTimer = Random.Range(.5f, .75f);

        sm.GetAudioCall().CallAudioClip("Travel");
    }

    public override void thisUpdate()
    {
        base.thisUpdate();
        movementTimer -= Time.deltaTime;
        if (movementTimer<= 0) { sm.ChangeState(sm.InitialState()); }
    }

    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();
        sm.GetRigidbody2D().linearVelocity = movementDirection * sm.GetMovementSpeed();
    }

    public override void thisEnd()
    {
        base.thisEnd();
        sm.GetRigidbody2D().linearVelocity = Vector2.zero;
    }
}
