using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCombat : MonoBehaviour
{
    PlayerStateMachine sm;

    [Header("References")]
    public GameObject bulletObj;
    public GameObject bombObj;
    public GameObject swordObj;     //Gets the sword game object
    public Sword swordHitBox;       //Gets the Sword script from the sword game object

    [Header("Timing")]
    public float swordActiveTime = 0.12f;
    public float swordCooldown = 0.25f;
    private bool canSwordAttack = true;

    [Header("Supplies Count")]
    private int bombCount = 5;
    private int ammoCount = 10;
    private SetHUDText HUDtextDisplay;

    [Header("Positioning")]
    public float swordOffset = 1f;

    private void Awake()
    {
        sm = GetComponent<PlayerStateMachine>();

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
        if (ammoCount <= 0) { return; }
        else { ammoCount--; }

        HUDtextDisplay.SetAmmoText(ammoCount);

        if (bulletObj)
        {
            Debug.Log("Spawning bullet");
            Instantiate(bulletObj, transform.position, transform.rotation)
            .GetComponent<Bullet>()
            .SetDirection(sm.GetFacingDirection());
        }
        
    }

    //Handles the bomb button
    public void Bomb()
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

        ApplyFacingToSword(sm.GetFacingDirection());

        swordHitBox.BeginSwing();
        swordObj.SetActive(true);

        yield return new WaitForSeconds(swordActiveTime);

        swordObj.SetActive(false);

        //yield return new WaitForSeconds(swordCooldown); 
            // I'm removing the sword cool-down because I think it feels better to not have it, though maybe we can re-implement it later -Dirk
        canSwordAttack = true;
    }

    //Controls the sword appearance based on the direction the player is "facing" / moving in
    private void ApplyFacingToSword(PlayerStateMachine.facingDirection facingDirection)
    {
        Vector3 localPos = Vector2.up * swordOffset;
        float zRot = 0f;

        switch (facingDirection)
        {
            case PlayerStateMachine.facingDirection.up:
                // values are set to up by default, this code simply returns without setting anything
                break;
            case PlayerStateMachine.facingDirection.down:
                localPos = Vector2.down * swordOffset;
                zRot = 180f;
                break;
            case PlayerStateMachine.facingDirection.left:
                localPos = Vector2.left * swordOffset;
                zRot = 90f;
                break;
            case PlayerStateMachine.facingDirection.right:
                localPos = Vector2.right * swordOffset;
                zRot = -90f;
                break;
        }

        swordObj.transform.localPosition = localPos;
        swordObj.transform.localRotation = Quaternion.Euler(0f, 0f, zRot);
    }

}
