using UnityEngine;

public class FinalBoss2StateRun : StateBase
{
    FinalBoss2SM sm;

    public FinalBoss2StateRun(StateMachineBase _sm) : base(_sm)
    {
        sm = (FinalBoss2SM)_sm;
    }

    Vector2 movementDirection;

    public Vector2 GetMovementDirection() { return movementDirection; }

    float recalculateTimer;
    float shootTimer;

    public override void thisStart()
    {
        base.thisStart();
        recalculateTimer = 0;
        shootTimer = 3f;

        sm.GetAnimator().Play("FB2_Walk");
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        recalculateTimer -= Time.deltaTime;

        if (recalculateTimer <= 0)
        {
            recalculateTimer = Random.Range(.75f, 2f);

            int a = Random.Range(0, 4);

            switch (a)
            {
                case 0:
                    movementDirection = Vector2.down;

                    if (Physics2D.Raycast(sm.transform.position, Vector2.down, 2f, LayerMask.GetMask("Walls", "Destructible")))
                    {
                        movementDirection = Vector2.up;
                    }

                    return;
                case 1:
                    sm.transform.localScale = new Vector3(-1, 1, 1);
                    movementDirection = Vector2.left;

                    if (Physics2D.Raycast(sm.transform.position, Vector2.left, 2f, LayerMask.GetMask("Walls", "Destructible")))
                    {
                        sm.transform.localScale = new Vector3(1, 1, 1);
                        movementDirection = Vector2.right;
                    }

                    return;
                case 2:
                    movementDirection = Vector2.up;

                    if (Physics2D.Raycast(sm.transform.position, Vector2.up, 2f, LayerMask.GetMask("Walls", "Destructible")))
                    {
                        movementDirection = Vector2.down;
                    }

                    return;
            }


            sm.transform.localScale = new Vector3(1, 1, 1);
            movementDirection = Vector2.right;

            if (Physics2D.Raycast(sm.transform.position, Vector2.right, 2f, LayerMask.GetMask("Walls", "Destructible")))
            {
                sm.transform.localScale = new Vector3(-1, 1, 1);
                movementDirection = Vector2.left;
            }

            return;
        }

        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0) { sm.ChangeState(sm.stateShoot); }
    }

    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();

        sm.GetRigidbody2D().MovePosition(sm.transform.position + (Vector3)movementDirection * sm.GetMovementSpeed() * Time.fixedDeltaTime);
    }
}
