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

    [Header("Unlocks")]
    public bool gunUnlocked = false;    //Call the function UnlockGun() to unlock the shooting mechanic
    public bool bombUnlocked = false;   //Call the function UnlockBomb() to unlock the bomb mechanic
    public bool shieldUnlocked = false; //Call the function UnlockShield() to unlock the shield mechanic

    [Header("JoyStick position")] 
    public Vector2 moveInput;

    [Header("Timing")]
    public float swordActiveTime = 0.12f;
    public float swordCooldown = 0.25f;
    private bool canSwordAttack = true;

    [Header("Supplies Count")]
    private int bombCount = 5;
    private int ammoCount = 10;
    private SetHUDText HUDtextDisplay;

    [Header("Positioning")]
    public Vector2 offsetRight = new Vector2(0.6f, 0f); //Where the object appears when player is facing right
    public Vector2 offsetLeft = new Vector2(-0.6f, 0f);  //Where the object appears when player is facing left
    public Vector2 offsetUp = new Vector2(0f, 0.6f);    //Where the object appears when player is facing up
    public Vector2 offsetDown = new Vector2(0f, -0.6f);  //Where the object appears when player is facing down

    private void Awake()
    {
        HUDtextDisplay = FindFirstObjectByType<SetHUDText>();
        HUDtextDisplay.SetAmmoText(ammoCount);
        HUDtextDisplay.SetBombText(bombCount);
    }

    // attach combat functions to their respective button inputs in InputManager.cs
    private void OnEnable()
    {
        InputManager.shootInput += Shoot;
        InputManager.bombInput += Bomb;
        InputManager.meleeInput += SwordAttack;
    }

    // detach combat functions from their respective button inputs in InputManager.cs to avoid memory leaks
    private void OnDisable()
    {
        InputManager.shootInput -= Shoot;
        InputManager.bombInput -= Bomb;
        InputManager.meleeInput -= SwordAttack;
    }

    //Handles the shoot button
    public void Shoot()
    {
        //Locks the gun until the player unlocks it
        if (gunUnlocked ==  true)
        {
            if (ammoCount <= 0) { return; }
            else { ammoCount--; }

            HUDtextDisplay.SetAmmoText(ammoCount);

            if (bulletObj)
            {
                Debug.Log("Spawning bullet");
                Instantiate(bulletObj, transform.position, transform.rotation)
                .GetComponent<Bullet>()
                .SetDirection(InputManager.facingDirection);
            }
        }
        
        
    }

    //Handles the bomb button
    public void Bomb()
    {
        //Locks the bomb until the player unlocks it
        if (bombUnlocked == true)
        {
            if (bombCount <= 0) { return; }
            else { bombCount--; }

            HUDtextDisplay.SetBombText(bombCount);

            if (bombObj)
            {
                Debug.Log("Spawning bomb");
                Instantiate(bombObj, transform.position, Quaternion.identity);
            }
        }
        
    }

    //Handles the sword button
    public void SwordAttack()
    {
        //Debug.Log("Sword attack trigger");
        if (!canSwordAttack) return;
        StartCoroutine(SwingSword());
    }

    //Coroutine for swinging the sword
    IEnumerator SwingSword()
    {
        canSwordAttack = false;

        ApplyFacingToSword(InputManager.facingDirection);

        swordHitBox.BeginSwing();
        swordObj.SetActive(true);

        yield return new WaitForSeconds(swordActiveTime);

        swordObj.SetActive(false);

        //yield return new WaitForSeconds(swordCooldown); 
            // I'm removing the sword cool-down because I think it feels better to not have it, though maybe we can re-implement it later -Dirk
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
                Debug.Log("Sword right");
                localPos = offsetRight;
                zRot = -90f;
            }
            else
            {
                Debug.Log("Sword left");
                localPos = offsetLeft;
                zRot = 90f;
            }
        }
        else
        {
            // Up / Down
            if (direction.y >= 0f)
            {
                Debug.Log("Sword up");
                localPos = offsetUp;
                zRot = 0f;
            }
            else
            {
                Debug.Log("Sword down");
                localPos = offsetDown;
                zRot = 180f;
            }
        }

        swordObj.transform.localPosition = localPos;
        swordObj.transform.localRotation = Quaternion.Euler(0f, 0f, zRot);
    }

    //Handles unlocks for gun, bomb, and shield
    public void UnlockGun()
    {
        gunUnlocked = true;
        Debug.Log("Gun unlocked!");
    }

    public void UnlockBomb()
    {
        bombUnlocked = true;
        Debug.Log("Bomb unlocked!");
    }

    public void UnlockShield()
    {
        shieldUnlocked = true;
        Debug.Log("Shield unlocked!");
    }

}
