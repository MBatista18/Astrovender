using UnityEngine;

public class FinalStateP2_Rocket : StateBase
{
    FinalBossSM sm;

    public FinalStateP2_Rocket(StateMachineBase _sm) : base(_sm)
    {
        sm = (FinalBossSM)_sm;
    }

    float movementY;

    float movementX;

    float timer;

    public override void thisStart()
    {
        base.thisStart();

        sm.GetAudioCall().CallAudioClip("Rush");

        float a = Physics2D.Raycast(sm.transform.position, Vector2.down, 20f, LayerMask.GetMask("Destructible", "Walls")).distance;
        float b = Physics2D.Raycast(sm.transform.position, Vector2.up, 20f, LayerMask.GetMask("Destructible", "Walls")).distance;

        movementY = a > b ? -1 : 1;


        float c = Physics2D.Raycast(sm.transform.position, Vector2.left, 20f, LayerMask.GetMask("Destructible", "Walls")).distance;
        float d = Physics2D.Raycast(sm.transform.position, Vector2.right, 20f, LayerMask.GetMask("Destructible", "Walls")).distance;

        movementX = c > d ? -1 : 1;

        timer = 10f;
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        RaycastHit2D stopCheck = Physics2D.BoxCast(sm.transform.position + (Vector3)(Vector2.right * movementX * sm.GetCollider2D().size.x / 2),
            new Vector2(.1f, sm.GetCollider2D().size.y * .95f),
            0f, Vector2.zero, 0f, LayerMask.GetMask("Destructible", "Walls"));

        if (stopCheck)
        {
            sm.GetAudioCall().CallAudioClip("Collide2");
            AssetCall.instance.cameraEffectors.SetCameraShake(.7f);
            sm.ChangeState(sm.stateP2_Pause);
        }

        RaycastHit2D flipCheck = Physics2D.BoxCast(sm.transform.position + (Vector3)(Vector2.up * movementY * sm.GetCollider2D().size.x / 2),
            new Vector2(sm.GetCollider2D().size.x, .1f),
            0f, Vector2.zero, 0f, LayerMask.GetMask("Destructible", "Walls"));

        if (flipCheck)
        {
            sm.GetAudioCall().CallAudioClip("Collide");

            AssetCall.instance.cameraEffectors.SetCameraShake(0.1f);

            movementY = -movementY;

            sm.GetAnimator().Play(movementY < 0 ? "FBP2_Forward" : "FBP2_Backward");
        }

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            sm.ChangeState(sm.stateP2_Pause);
        }
    }

    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();

        Vector3 newPos = sm.transform.position + new Vector3(.25f * movementX, movementY).normalized * sm.GetMovementSpeed() * 6 * Time.fixedDeltaTime;

        sm.GetRigidbody2D().MovePosition(newPos);
    }
}
