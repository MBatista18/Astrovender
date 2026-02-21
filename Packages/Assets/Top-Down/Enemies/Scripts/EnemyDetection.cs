using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRadius = 5f;
    private Vector3 originalPosition;
    private bool isPlayerDetected = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 targetPosition = isPlayerDetected ? player.position : originalPosition;
        MoveToPosition(targetPosition);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == player)
        {
            Debug.Log("Player detected!");
            isPlayerDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == player)
        {
            Debug.Log("Player escaped!");
            isPlayerDetected = false;
        }
    }

    private void MoveToPosition(Vector3 targetPosition)
    {
        Vector2 direction = (targetPosition - transform.position).normalized;

        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }

     private void OnDrawGizmos()
    {
        // Visualize the detection radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
