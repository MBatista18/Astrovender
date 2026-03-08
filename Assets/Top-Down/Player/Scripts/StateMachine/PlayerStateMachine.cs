using UnityEngine;

public class PlayerStateMachine : StateMachineBase
{
    #region variables

    [SerializeField] private float _moveSpeed = 5f;
    public float GetSpeed() { return _moveSpeed; }

    private Vector2 _movement;
    public Vector2 GetMovement() { return _movement; }
    public void SetMovement(Vector2 movementDirection)
    {
        Vector2 referenceDirection = Vector2.zero;

        // the following is designed to modify movementDirection for octagonal movement input

        if (Mathf.Abs(movementDirection.y) > 0.2f) // checks y against a certain threshold; if it passes that threshold, the player moves vertically
        {
            referenceDirection += new Vector2(0, 1);
        }

        if (Mathf.Abs(movementDirection.x) > 0.2f) // checks x against a certain threshold; if it passes that threshold, the player moves horizontally
        {
            referenceDirection += new Vector2(1, 0);
        }

        referenceDirection = new Vector2(referenceDirection.x * Mathf.Sign(movementDirection.x), referenceDirection.y * Mathf.Sign(movementDirection.y));
            // applies the directional vallue of movementDirection to referenceDirection

        _movement = referenceDirection.normalized; // normalizes the vector to prevent diagonal movement  speeds exceeding straight horizontal or straight vertical movement speeds

        SetFacingDirection();
    }

    // the following values modify the player's facing direction
    private void SetFacingDirection()
    {
        if (_movement.Equals(Vector2.zero)) { return; } // don't change the facing direction if the player is not moving

        if (Mathf.Abs(_movement.x) > Mathf.Abs(_movement.y))
        {
            if (_movement.x > 0)
            {
                base.SetFacingDirection(AstrovenderStructs.facingDirection.right);
            }
            else if (_movement.x < 0)
            {
                base.SetFacingDirection(AstrovenderStructs.facingDirection.left);
            }
        }
        else
        {
            if (_movement.y > 0)
            {
                base.SetFacingDirection(AstrovenderStructs.facingDirection.up);
            }
            else if (_movement.y < 0)
            {
                base.SetFacingDirection(AstrovenderStructs.facingDirection.down);
            }
        }

        animationController.Animate(); // changes the animation controller's current facing direction
    }

    private int currentKeyCount;
    public int GetKeyCount() { return currentKeyCount; }
    public void CollectKey() { currentKeyCount++; }
    public void UseKey() { currentKeyCount--; }

    #endregion

    #region components

    private Rigidbody2D rb;
    public Rigidbody2D GetRB2d() { return rb; }

    private PlayerAnimationController animationController;
    public PlayerAnimationController GetAnimationController() { return animationController; }

    private PlayerHealth playerHealth;
    public PlayerHealth GetPlayerHealth() { return playerHealth; }

    public override void InstantiateComponents()
    {
        base.InstantiateComponents();
        rb = GetComponent<Rigidbody2D>();
        animationController = GetComponent<PlayerAnimationController>();
        playerHealth = GetComponent<PlayerHealth>();
    }
    #endregion

    #region states

    PlayerStateIdle stateIdle;
    public override StateBase InitialState()
    {
        return stateIdle;
    }

    PlayerStateMove stateMove;
    public PlayerStateMove GetStateMove() { return stateMove; }

    PlayerStateKnockback stateKnockback;
    public void Knockback(Vector2 callerPosition, float time)
    {  
        // knocks the player either horizontally or vertically (horizontally if they're farther on the x axis, verticaly if they're farther on the y-axis)

        Vector2 distanceDiff = new Vector2(transform.position.x - callerPosition.x, transform.position.y - callerPosition.y).normalized;

        Vector2 knockDirection =
            Mathf.Abs(distanceDiff.x) > Mathf.Abs(distanceDiff.y) ? Vector2.right * Mathf.Sign(distanceDiff.x) : Vector2.up * Mathf.Sign(distanceDiff.y);

        stateKnockback.SetKnockback(knockDirection, time);
        ChangeState(stateKnockback);
    }

    PlayerStateDead stateDead;
    public override StateBase DeathState()
    {
        return stateDead;
    }

    public override void InstantiateStates()
    {
        base.InstantiateStates();
        stateIdle = new PlayerStateIdle(this);
        stateMove = new PlayerStateMove(this);
        stateKnockback = new PlayerStateKnockback(this);
        stateDead = new PlayerStateDead(this);
    }

    #endregion

    public override void UpdateFunctions()
    {
        base.UpdateFunctions();

        SetMovement(new Vector2(InputManager.Movement.x, InputManager.Movement.y));
    }
}
