using UnityEngine;

public class BombTankStateRush : StateBase
{
    BombTankBossSM sm;
    public BombTankStateRush(StateMachineBase _sm) : base(_sm)
    {
        sm = (BombTankBossSM)_sm;
    }

    float timer;
    Vector2 moveDirection;

    public override void thisStart()
    {
        base.thisStart();
        timer = 1f;

        if (Random.Range(0, 10) < 7) { Object.Instantiate(sm.GetBomb(), sm.transform.position, Quaternion.identity); }

        switch (sm.GetStateMove().movedHorizontally)
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

                break;
        }

        sm.SetReactToDamage(false);
    }

    public override void thisUpdate()
    {
        base.thisUpdate();
        timer -= Time.deltaTime;

        if (timer <= 0) { sm.ChangeState(sm.InitialState()); }
    }

    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();

        sm.GetRigidbody2D().linearVelocity = moveDirection * sm.GetMovementSpeed() * 2;
    }

    public override void thisEnd()
    {
        base.thisEnd();
        sm.SetReactToDamage(true);
    }
}
