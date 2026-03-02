using UnityEngine;

public class EnemyStatePatrol : StateBase
{
    EnemySM sm;

    public EnemyStatePatrol(StateMachineBase _sm) : base(_sm) 
    {
        sm = (EnemySM) _sm;
    }

    Vector3 startingPosition; // position where the enemy starts during this state.

    Vector3 targetPosition;

    bool isMoving;

    public override void thisStart()
    {
        base.thisStart();


        startingPosition = sm.transform.position;

        isMoving = false; // idles for 0.7f seconds upon start;
        updateTimer = 0.7f;
    }

    float updateTimer;

    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();

        CheckForPlayer();

        updateTimer -= Time.fixedDeltaTime;

        if (isMoving)
        {
            UpdateMoving();
        }
        else
        {
            UpdateNotMoving();
        }
    }

    void CheckForPlayer()
    {
        if (Vector2.Distance(sm.GetPlayerTransform().position, sm.transform.position) <= sm.GetDetectionRadius()) 
        {
            sm.ChangeState(sm.AttackState());
        }
    }

    void UpdateNotMoving()
    {
        if (updateTimer > 0) { return; } // idle until the timer reaches 0

        targetPosition = startingPosition + (Vector3) (sm.GetPatrolRadius() * Random.insideUnitCircle);

        isMoving = true;

        updateTimer = Random.Range(1.8f, 3f);
    }

    void UpdateMoving()
    {
        sm.GetRigidbody2D().MovePosition(Vector3.MoveTowards(sm.GetRigidbody2D().position, targetPosition, sm.GetMovementSpeed() * Time.fixedDeltaTime));

        if (Vector3.Distance(sm.transform.position, targetPosition) > 0.1f && updateTimer > 0) { return; } // move until close to the target or the timer reaches 0

        isMoving = false;

        updateTimer = Random.Range(1.8f, 3f);
    }
}
