using System;
using UnityEngine;

// A debug class used to handle player information
public class PlayerStatusTest : MonoBehaviour
{
    public int health = 0;

    public KeyCode failureState = KeyCode.Z;
    public KeyCode successState = KeyCode.X;
    public KeyCode gainResources = KeyCode.Comma;
    public KeyCode loseResources = KeyCode.Period;

    public event Action<bool> OnPlayerStatusUpdate; // true - survive, false - dead

    private void Update()
    {
       /* if (Input.GetKeyDown(failureState))
        {
            Debug.Log("Failure State Activated");
            OnPlayerStatusUpdate.Invoke(false);
        }
        else if (Input.GetKeyDown(successState))
        {
            Debug.Log("Success State Activated");
            OnPlayerStatusUpdate.Invoke(true);
        }
        else if (Input.GetKeyDown(gainResources))
        {
            GameManager.Instance.IncrementCoins(1);
            Debug.Log($"Gained Resources.");
        }
        else if (Input.GetKeyDown(loseResources))
        {
            GameManager.Instance.IncrementCoins(-1);
            Debug.Log($"Reduced Resources.");
        }*/
    }
}
