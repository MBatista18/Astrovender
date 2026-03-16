using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootableButton : EnemySM
{
    [Header("Bridge Script References")]
    [SerializeField] SlidingBridge slidingBridge;
    [SerializeField] ShootOpenDoor shootOpenDoor;

    [Header("Button Settings")]
    //[SerializeField] private bool isTimed = false;

    [Header("Button Feedback")]
    [SerializeField] private SpriteRenderer buttonRenderer;
    //    [SerializeField] private Color activatedColor = Color.green;
    [SerializeField] Sprite activatedSprite;

    public override void PainReactions()
    {
        ActivateButton();
    }

    //Handles button activation logic
    public void ActivateButton()
    {
        Debug.Log("ActivateButton called on: " + gameObject.name);

        //Determines which bridge it controls depending on what it is set for
      /*  if (isTimed)
        {
            timedBridge.PauseBridge();
        }
        else
        {
            slidingBridge.SlidingButton();
        } */

        slidingBridge?.SlidingButton();
        shootOpenDoor?.OpenDoor();

        //Changes the button color upon activation
        if (buttonRenderer != null)
        {
            buttonRenderer.sprite = activatedSprite;
            //buttonRenderer.color = activatedColor;
        }
    }
}
