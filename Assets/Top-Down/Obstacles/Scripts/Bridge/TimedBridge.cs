using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimedBridge : MonoBehaviour
{
    //Movement points can simply use the bridge start/end position prefabs
    [Header("Movement Points")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2.0f;

    [Header("Pause Settings")]
    [SerializeField] private float pauseDuration = 2.0f;

    private Vector3 targetPosition;
    private bool isPaused = false;
    private Coroutine pauseRoutine;

    private void Start()
    {
        //Checks if points A or B are valid
        if (pointA == null || pointB == null)
        {
            Debug.LogError("TimedBridge: Missing movement points.");
            enabled = false;
            return;
        }

        transform.position = pointA.position;
        targetPosition = pointB.position;
    }

    private void Update()
    {
        //If paused, stop bridge from moving
        if (isPaused) {return;}
            
        MoveBridge();
    }

    private void MoveBridge()
    {
        //Moves the bridge towards target point
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        //Changes target destination once previous destination was reached
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            targetPosition = (targetPosition == pointA.position)
                ? pointB.position
                : pointA.position;
        }
    }

    // This is the function the button will call
    public void PauseBridge()
    {
        if (pauseRoutine != null)
            StopCoroutine(pauseRoutine);

        pauseRoutine = StartCoroutine(PauseCoroutine());
    }

    //Pauses the bridge
    private IEnumerator PauseCoroutine()
    {
        isPaused = true;

        yield return new WaitForSeconds(pauseDuration);

        isPaused = false;
    }

}