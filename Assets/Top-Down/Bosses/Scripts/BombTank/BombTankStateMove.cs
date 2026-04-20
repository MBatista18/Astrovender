using UnityEngine;

public class BombTankStateMove : StateBase
{
    BombTankBossSM sm;
    public BombTankStateMove(StateMachineBase _sm) : base(_sm)
    {
        sm = (BombTankBossSM)_sm;
    }

    float moveDuration;

    Vector2 moveDirection = Vector2.zero;
    public bool movedHorizontally = true; // set to be true by default so it begins by moving vertically

    public override void thisStart()
    {
        base.thisStart();

        sm.GetAudioCall().CallAudioClip("Move");

        moveDuration = Random.Range(1.5f, 2.5f);

        switch (movedHorizontally)
        {
            case false:
                float right = Physics2D.Raycast(sm.transform.position, Vector2.right, 40f, LayerMask.GetMask("Walls")).distance;
                float left = Physics2D.Raycast(sm.transform.position, Vector2.left, 40f, LayerMask.GetMask("Walls")).distance;

                if (right > left)
                {
                    moveDirection = Vector2.right;
                }
                else
                {
                    moveDirection = Vector2.left;
                }

                movedHorizontally = true;

                break;
            case true:
                float up = Physics2D.Raycast(sm.transform.position, Vector2.up, 40f, LayerMask.GetMask("Walls")).distance;
                float down = Physics2D.Raycast(sm.transform.position, Vector2.down, 40f, LayerMask.GetMask("Walls")).distance;

                if (up > down)
                {
                    moveDirection = Vector2.up;
                }
                else
                {
                    moveDirection = Vector2.down;
                }

                movedHorizontally = false;

                break;
        }

        sm.GetAudioCall().CallAudioClip("BombPlace");
        Object.Instantiate(sm.GetBomb(), sm.transform.position, Quaternion.identity);
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        moveDuration -= Time.deltaTime;

        if (moveDuration <= 0)
        {
            sm.ChangeState(sm.InitialState());
        }

        bool check = Physics2D.BoxCast(sm.transform.position + ((Vector3)moveDirection * .5f), new Vector2(1.7f, 1.7f), 0f, Vector2.zero, 0f, LayerMask.GetMask("Walls"));

        if (check)
        {
            moveDuration = 0;
        }
    }

    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();

        sm.GetRigidbody2D().linearVelocity = moveDirection * sm.GetMovementSpeed();
    }

    public override void thisEnd()
    {
        base.thisEnd();

        sm.GetRigidbody2D().linearVelocity = Vector2.zero;
    }
}
