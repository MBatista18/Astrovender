using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 Movement;

    private PlayerInput _playerInput;
    private InputAction _moveAction;

    // static events that activate upon each input, and then call their respective actions from PlayerCombat.cs
    public delegate void OnInput();
    public static OnInput bombInput;
    public static OnInput shootInput;
    public static OnInput meleeInput;
    public static OnInput mapInput;
    public static OnInput pauseInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        _moveAction = _playerInput.actions["Move"];
    }

    private void Update()
    {
        if (Time.timeScale == 0) { return; }
        Movement = _moveAction.ReadValue<Vector2>();

        if (Movement == Vector2.zero)
        {
            return;
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

    void OnMap()
    {
        mapInput();
    }

    void OnPause()
    {
        pauseInput();
    }
}
