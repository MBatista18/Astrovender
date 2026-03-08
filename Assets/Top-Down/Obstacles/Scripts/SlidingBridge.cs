using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlidingBridge : MonoBehaviour
{
    [Header("Bridge Positions")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3.0f;

    private bool isMoving = false;
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

    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving) { return; }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
            );

        if (Vector3.Distance(transform.position, targetPosition ) < 0.01f )
        {
            transform.position = targetPosition;
            isMoving = false;
        }

    }

    //Tells the bridge where to extend/retract and sets isMoving to true
    public void ExtendBridge()
    {
        targetPosition = endPoint.position;
        isMoving = true;
    }

    public void RetractBrirdge()
    {
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
