using UnityEngine;

public class FlyingEyeAI : MonoBehaviour
{
    private enum State { Patrol, Chase }
    [SerializeField] private State state = State.Patrol;

    [Header("References")]
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] Transform playerTransform;

    [Header("Movement")]
    [Tooltip("If empty, will wander around start position.")]
    [SerializeField] private float patrolSpeed = 2.0f;
    [SerializeField] private float chaseSpeed = 3.0f;
    [SerializeField] private float arriveDistance = 0.15f;

    [Header("Patrol")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] float wanderRadius = 3.0f;
    [SerializeField] private float wanderPointHoldTime = 0.5f;

    [Header("Detection")]
    [SerializeField] private float losePlayerDistance = 6.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
