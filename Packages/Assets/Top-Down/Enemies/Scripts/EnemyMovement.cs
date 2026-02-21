using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveTime = 2f;
    private float patrolRadius = 3f;

    private Vector2 moveDirection;
    private int currentDirection = -1;
    private float moveTimer;
    private Vector2 startingPosition;

    private void Start()
    {
        startingPosition = transform.position;  // Store starting point for patrol radius
        PickNewDirection();
    }

    private void Update()
    {
        // Move the enemy
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Countdown timer
        moveTimer -= Time.deltaTime;

        if (moveTimer <= 0)
        {
            PickNewDirection();
        }
    }

    private void PickNewDirection()
    {
        int newDirection;
        int opposite = currentDirection == -1 ? -1 : GetOppositeDirection(currentDirection);

        int attempts = 0;
        bool validDirectionFound = false;

        while(!validDirectionFound && attempts < 20)
        {
            newDirection = Random.Range(0, 8);
            moveDirection = DirectionToVector(newDirection);

            Vector2 predictedPosition = (Vector2)transform.position + moveDirection * moveSpeed * moveTime;

            if(newDirection != opposite && Vector2.Distance(startingPosition, predictedPosition) <= patrolRadius)
            {
                validDirectionFound = true;
                currentDirection = newDirection;
            }

            attempts ++;
        }

        moveTimer = moveTime;
    }

    // Converts an integer direction to a Vector2
    private Vector2 DirectionToVector(int dir)
    {
        switch (dir)
        {
            case 0: return Vector2.up;
            case 1: return Vector2.down;
            case 2: return Vector2.left;
            case 3: return Vector2.right;
            case 4: return new Vector2(1, 1).normalized;
            case 5: return new Vector2(-1, 1).normalized;
            case 6: return new Vector2(1, -1).normalized;
            case 7: return new Vector2(-1, -1).normalized;
        }
        return Vector2.zero;
    }

    // Returns the opposite direction for the given direction
    private int GetOppositeDirection(int direction)
    {
        switch (direction)
        {
            case 0: return 1;
            case 1: return 0;
            case 2: return 3;
            case 3: return 2;
            case 4: return 7;
            case 5: return 6;
            case 6: return 5;
            case 7: return 4;
        }
        return direction;
    }

    private void OnDrawGizmos()
    {
        // Visualize the detection radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}