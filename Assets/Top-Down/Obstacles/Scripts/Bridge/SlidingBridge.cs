using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlidingBridge : MonoBehaviour
{
    [Header("Bridge Positions")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] Collider2D bridgeSpaceCollider;
    [SerializeField] LineRenderer lineRenderer;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3.0f;

    [Header("Bridge Settings")]
    [SerializeField] private bool useToggle = false;

    private bool isMoving = false;
    private bool hasBeenActivated = false;
    private Vector3 targetPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Checks to see if startPoint and endPoint are valid
        if (startPoint == null || endPoint == null)
        {
            Debug.LogError("SlidingBridge: Start point or end point is missing");
            return;
        }


        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);

        bridgeSpaceCollider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving) { return; }

        Debug.Log("line renderer pos = " + lineRenderer.GetPosition(1) + "; target = " + targetPosition);

        lineRenderer.SetPosition(1, Vector3.MoveTowards(
            lineRenderer.GetPosition(1),
            targetPosition,
            moveSpeed * Time.deltaTime
            ));

        if (Vector3.Distance(transform.position, targetPosition ) < 0.01f )
        {
            transform.position = targetPosition;
            isMoving = false;
        }

    }

    //The function that the button calls when shot
    public void SlidingButton()
    {
        //Checks if the bridge has already been activated
        if (hasBeenActivated == true) { return; }

        //Checks if the toggle option is set to true, if not, bridge is one time use
        if (useToggle)
        {
            ToggleBridge();
        }
        else
        {
            ExtendBridge();
        }

        //Setting hasBeenActivated to true so button can't be activated again
        hasBeenActivated = true;
        Debug.Log("SlidingBridge has been activated!");

    }

    //Tells the bridge where to extend/retract and sets isMoving to true
    public void ExtendBridge()
    {
        bridgeSpaceCollider.enabled = false;
        targetPosition = endPoint.position;
        isMoving = true;
    }

    public void RetractBrirdge()
    {
        bridgeSpaceCollider.enabled = false; //this probably causes a collision issue if the players on the bridge already
        targetPosition = startPoint.position;
        isMoving = true;
    }

    //Allows the player to press the button again to toggle the bridge
    public void ToggleBridge()
    {
        float distanceToEnd = Vector3.Distance(transform.position, endPoint.position);
        float distanceToStart = Vector3.Distance(transform.position, startPoint.position);

        if (distanceToEnd < distanceToStart)
        {
            RetractBrirdge();
        }
        else
        {
            ExtendBridge();
        }
    }

}
