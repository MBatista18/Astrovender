using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    public GameObject bulletObj;
    public GameObject bombObj;
    public GameObject swordObj;     //Gets the sword game object
    public Sword swordHitBox;       //Gets the Sword script from the sword game object

    [Header("JoyStick position")] 
    public Vector2 moveInput;
    private Vector2 lastFacing = Vector2.up;

    [Header("Timing")]
    public float swordActiveTime = 0.12f;
    public float swordCooldown = 0.25f;
    private bool canSwordAttack = true;

    [Header("Positioning")]
    public Vector2 offsetRight = new Vector2(0.6f, 0f); //Where the object appears when player is facing right
    public Vector2 offsetLeft = new Vector2(-0.6f, 0f);  //Where the object appears when player is facing left
    public Vector2 offsetUp = new Vector2(0f, 0.6f);    //Where the object appears when player is facing up
    public Vector2 offsetDown = new Vector2(0f, -0.6f);  //Where the object appears when player is facing down

    // Update is called once per frame
    void Update()
    {
        //Updates whenever the joystick is pushed
        if(moveInput.sqrMagnitude > 0.01f)
        {
            lastFacing = moveInput.normalized;
        }
    }

    //Handles the shoot button
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

    //Handles the bomb button
    public void Bomb()
    {
        Debug.Log("Bomb button pressed");

        if(bombObj)
        {
            Debug.Log("Spawning bomb");
            Instantiate(bombObj, transform.position, Quaternion.identity);
        }
        
    }

    //Handles the sword button
    public void SwordAttack()
    {
        Debug.Log("Sword attack trigger");
        if (!canSwordAttack) return;
        StartCoroutine(SwingSword());
    }

    //Coroutine for swinging the sword
    IEnumerator SwingSword()
    {
        canSwordAttack = false;

        ApplyFacingToSword(lastFacing);

        swordHitBox.BeginSwing();
        swordObj.SetActive(true);

        yield return new WaitForSeconds(swordActiveTime);

        swordObj.SetActive(false);

        yield return new WaitForSeconds(swordCooldown);
        canSwordAttack = true;
    }

    //Controls the sword appearance based on the direction the player is "facing" / moving in
    private void ApplyFacingToSword(Vector2 direction)
    {
        direction = direction.normalized;

        Vector3 localPos = offsetRight;
        float zRot = 0f;

        // Snap to cardinal direction based on whichever axis is strongest (Can't test without joystick)
        if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
        {
            // Left / Right
            if (direction.x >= 0f)
            {
                localPos = offsetRight;
                zRot = 0f;
            }
            else
            {
                localPos = offsetLeft;
                zRot = 180f;
            }
        }
        else
        {
            // Up / Down
            if (direction.y >= 0f)
            {
                localPos = offsetUp;
                zRot = 90f;
            }
            else
            {
                localPos = offsetDown;
                zRot = -90f;
            }
        }

        swordObj.transform.localPosition = localPos;
        swordObj.transform.localRotation = Quaternion.Euler(0f, 0f, zRot);
    }

}
