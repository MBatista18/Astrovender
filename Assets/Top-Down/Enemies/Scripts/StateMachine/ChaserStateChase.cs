using UnityEngine;

public class ChaserStateChase : StateBase
{
    ChaserSM sm;

    bool playAudio;

    public ChaserStateChase(StateMachineBase _sm) : base(_sm)
    {
        sm = (ChaserSM) _sm;
        playAudio = true;
    }

    public override void thisStart()
    {
        base.thisStart();

        switch (sm.facingDirection)
        {
            case AstrovenderStructs.facingDirection.up:
                sm.GetAnimator().Play("GreyWalkForward");
                break;
            case AstrovenderStructs.facingDirection.down:
                sm.GetAnimator().Play("GreyWalkBackward");
                break;
            case AstrovenderStructs.facingDirection.right:
                sm.GetAnimator().Play("GreyWalkLeft");
                break;
            case AstrovenderStructs.facingDirection.left:
                sm.GetAnimator().Play("GreyWalkRight");
                break;
        }

        if (!playAudio) { return; }
        playAudio = false;
        sm.GetAudioSource().Play();
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        if (Mathf.Abs(AssetCall.instance.playerSM.transform.position.x-sm.transform.position.x) > Mathf.Abs(AssetCall.instance.playerSM.transform.position.y - sm.transform.position.y))
        {
            if (AssetCall.instance.playerSM.transform.position.x <= sm.transform.position.x)
            {
                if (sm.facingDirection != AstrovenderStructs.facingDirection.right)
                {
                    sm.GetAnimator().Play("GreyWalkLeft");
                }

                sm.facingDirection = AstrovenderStructs.facingDirection.right;
            }
            else
            {
                if (sm.facingDirection != AstrovenderStructs.facingDirection.left)
                {
                    sm.GetAnimator().Play("GreyWalkRight");
                }

                sm.facingDirection = AstrovenderStructs.facingDirection.left;
            }
        }
        else
        {
            if (AssetCall.instance.playerSM.transform.position.y <= sm.transform.position.y)
            {
                if (sm.facingDirection != AstrovenderStructs.facingDirection.up)
                {
                    sm.GetAnimator().Play("GreyWalkForward");
                }

                sm.facingDirection = AstrovenderStructs.facingDirection.up;
            }
            else
            {
                if (sm.facingDirection != AstrovenderStructs.facingDirection.down)
                {
                    sm.GetAnimator().Play("GreyWalkBackward");
                }

                sm.facingDirection = AstrovenderStructs.facingDirection.down;
            }
        }
    }

    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();

        if (Vector2.Distance(sm.GetPlayerTransform().position, sm.transform.position) > sm.GetDetectionRadius() * 1.5f)
            // if the player leaves the detection radius (buffer of 1.5 added to detection radius to prevent player from just marginally leaving radius to attack again)
        {
            playAudio = true;
            sm.ChangeState(sm.InitialState());
        }

        sm.GetRigidbody2D().MovePosition(Vector2.MoveTowards(sm.GetRigidbody2D().position, sm.GetPlayerTransform().position, sm.GetMovementSpeed() * Time.fixedDeltaTime));
    }
}
