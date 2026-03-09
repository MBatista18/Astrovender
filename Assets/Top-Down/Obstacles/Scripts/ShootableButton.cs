using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootableButton : EnemySM
{
    [Header("Bridge Reference")]
    [SerializeField] private SlidingBridge bridge;

    [Header("Button Settings")]
    [SerializeField] private bool useToggle = false;

    [Header("Button Feedback")]
    [SerializeField] private SpriteRenderer buttonRenderer;
    //    [SerializeField] private Color activatedColor = Color.green;
    [SerializeField] Sprite activatedSprite;

    private bool hasBeenActivated = false;

    public override void PainReactions()
    {
        ActivateButton();
    }

    //Handles button activation logic
    public void ActivateButton()
    {
        Debug.Log("ActivateButton called on: " + gameObject.name);

        //Checks if the button has already been activated
        if (hasBeenActivated == true) { return; }

        //Checks if bridge is valid
        if (bridge == null)
        {
            Debug.LogError("ShootableButton: bridge is missing on " + gameObject.name);
            return;
        }

        //Checks if the toggle option is set to true, if not, button is one time use
        if (useToggle)
        {
            bridge.ToggleBridge();
        }
        else
        {
            bridge.ExtendBridge();
        }

        //Setting hasBeenActivated to true so button can't be activated again
        hasBeenActivated = true;
        Debug.Log("Button has been activated!");

        //Changes the button color upon activation
        if (buttonRenderer != null)
        {
            buttonRenderer.sprite = activatedSprite;
            //buttonRenderer.color = activatedColor;
        }
    }
}
