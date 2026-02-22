using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 Movement;

    private PlayerInput _playerInput;
    private InputAction _moveAction;

    public static Vector2 facingDirection; // stores the player's facing direction, changing it relative to Movement
    
    // static events that activate upon each input, and then call their respective actions from PlayerCombat.cs
    public delegate void OnInput();
    public static OnInput bombInput;
    public static OnInput shootInput;
    public static OnInput meleeInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        _moveAction = _playerInput.actions["Move"];
    }

    private void Update()
    {
        Movement = _moveAction.ReadValue<Vector2>();

        if (Movement == Vector2.zero)
        {
            return;
        }

        if (Mathf.Abs(Movement.x) > Mathf.Abs(Movement.y)) // if the player is moving horizontally
        {
            facingDirection = Vector2.right * Mathf.Sign(Movement.x); // set facing direction to either be left or right depending on the x value of Movement
        }
        else // if the player is moving vertically or perfectly diagonally
        {
            facingDirection = Vector2.up * Mathf.Sign(Movement.y); // set facing direction to either be up or down depending on the y value of Movement
        }
    }

    void OnBomb()
    {
        bombInput();
    }

    void OnShoot()
    {
        shootInput();
    }

    void OnMelee()
    {
        meleeInput();
    }
}
