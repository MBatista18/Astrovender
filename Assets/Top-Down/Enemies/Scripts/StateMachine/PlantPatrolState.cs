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

        plant.rb.linearVelocity = Vector2.zero;
        plant.transform.position = plant.startPosition;

        isPeeking = false;
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        if (plant.PlayerInDetectionRange())
        {
            Debug.Log("Detecting player");
            if (!isPeeking)
            {
                isPeeking = true;
            }

            if (plant.PlayerInLungeRange())
            {
                plant.ChangeState(new PlantAttackState(plant));
            }
        }
        else
        {
            if (isPeeking)
            {
                isPeeking = false;
            }
        }
    }
}