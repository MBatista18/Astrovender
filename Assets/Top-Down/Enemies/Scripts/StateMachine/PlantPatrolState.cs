using UnityEngine;

public class PlantPatrolState : StateBase
{
    private PlantSM plant;
    private bool isPeeking;

    public PlantPatrolState(PlantSM sm) : base(sm)
    {
        plant = sm;
    }

    public override void thisStart()
    {
        base.thisStart();

        Debug.Log("Patrol");

        plant.rb.linearVelocity = Vector2.zero;
        plant.transform.position = plant.startPosition;

        isPeeking = false;
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        if (Vector3.Distance(AssetCall.instance.playerSM.transform.position, plant.transform.position) < plant.detectionRange)
        {
            //Debug.Log("Detecting player");
            if (!isPeeking)
            {
                isPeeking = true;
            }

            if (Vector3.Distance(AssetCall.instance.playerSM.transform.position, plant.transform.position) < plant.detectionRange/2)
            {
                plant.ChangeState(plant.AttackState());
            }
        }
        else
        {
            //Debug.Log("false");

            if (isPeeking)
            {
                isPeeking = false;
            }
        }
    }
}