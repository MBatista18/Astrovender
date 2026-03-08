using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    PlayerStateMachine sm;
    Animator animator;

    private void Awake()
    {
        sm = GetComponent<PlayerStateMachine>();
        animator = GetComponent<Animator>();
    }

    public enum AnimatorState
    {
        Idle,
        Walking
    }

    private AnimatorState currentAnimatorState;
    public void SetAnimatorState(AnimatorState state)
    {
        currentAnimatorState = state;

        Animate();
    }

    public void Animate()
    {
        string output = "";

        switch (currentAnimatorState)
        {
            case AnimatorState.Idle:
                output = IdleAnimations();
                break;
            case AnimatorState.Walking:
                output = MoveAnimations();
                break;
        }

        if (output.Equals("")) { return; }

        animator.Play(output);
    }

    string IdleAnimations()
    {
        string refName = "";

        switch (sm.GetFacingDirection())
        {
            case AstrovenderStructs.facingDirection.up:
                refName = "PlayerIdleUp";
                break;
            case AstrovenderStructs.facingDirection.down:
                refName = "PlayerIdleDown";
                break;
            case AstrovenderStructs.facingDirection.left:
                refName = "PlayerIdleLeft";
                break;
            case AstrovenderStructs.facingDirection.right:
                refName = "PlayerIdleRight";
                break;
        }

        return refName;
    }

    string MoveAnimations()
    {
        string refName = "";

        switch (sm.GetFacingDirection())
        {
            case AstrovenderStructs.facingDirection.up:
                refName = "PlayerMoveUp";
                break;
            case AstrovenderStructs.facingDirection.down:
                refName = "PlayerMoveDown";
                break;
            case AstrovenderStructs.facingDirection.left:
                refName = "PlayerMoveLeft";
                break;
            case AstrovenderStructs.facingDirection.right:
                refName = "PlayerMoveRight";
                break;
        }

        return refName;
    }
}
