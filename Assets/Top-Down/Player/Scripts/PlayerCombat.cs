using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject bulletObj;
    public GameObject bombObj;

    [Header("JoyStick position")]
    public Vector2 moveInput;
    private Vector2 lastFacing = Vector2.up;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //Updates whenever the joystick is pushed
        if(moveInput.sqrMagnitude > 0.01f)
        {
            lastFacing = moveInput.normalized;
        }
    }

    public void Shoot()
    {
        Debug.Log("Shoot button pressed");

        if (bulletObj)
        {
            Debug.Log("Spawning bullet");
            Instantiate(bulletObj, transform.position, transform.rotation)
            .GetComponent<Bullet>()
            .SetDirection(lastFacing);
        }
        
    }

    public void SwordAttack()
    {
        Debug.Log("Sword attack trigger");
    }

    public void Bomb()
    {
        Debug.Log("Bomb button pressed");

        if(bombObj)
        {
            Debug.Log("Spawning bomb");
            Instantiate(bombObj, transform.position, Quaternion.identity);
        }
        
    }
}
