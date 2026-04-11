using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootableButton : EnemySM
{

    [Header("Bridge Script References")]
    [SerializeField] SlidingBridge slidingBridge;
    [SerializeField] ShootOpenDoor shootOpenDoor;

    ObjectID objectID;

    public override void InstantiateComponents()
    {
        base.InstantiateComponents();
        objectID = GetComponent<ObjectID>();
    }

    public override StateBase InitialState()
    {
        if (GameManager.Instance.currentdataObj.saveENVGameWorld.Contains(objectID.GetID()))
        {
            PainReactions();
        }
        return base.InitialState();
    }

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
        //Debug.Log("ActivateButton called on: " + gameObject.name);

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

        GameManager.Instance.currentdataObj.saveENVGameWorld.Add(objectID.GetID());
    }
}
