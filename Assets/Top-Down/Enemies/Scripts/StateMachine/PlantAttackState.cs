using UnityEngine;

public class PlantAttackState : StateBase
{
    private PlantSM plant;

    private enum AttackPhase
    {
        Windup,
        Lunge,
        Recede,
        Cooldown
    }

    private AttackPhase phase;
    private float timer;

    public PlantAttackState(PlantSM sm) : base(sm)
    {
        plant = sm;
    }

    public override void thisStart()
    {
        base.thisStart();

        plant.rb.linearVelocity = Vector2.zero;

        phase = AttackPhase.Windup;
        timer = plant.peekDuration;
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        if (phase == AttackPhase.Windup)
        {
            timer -= Time.deltaTime;

            if (!plant.PlayerInLungeRange())
            {
                phase = AttackPhase.Recede;

                return;
            }

            if (timer <= 0f)
            {
                plant.lungeTarget = plant.GetClampedLungeTarget();
                phase = AttackPhase.Lunge;
            }
        }

        if (phase == AttackPhase.Cooldown)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                plant.ChangeState(new PlantPatrolState(plant));
            }
        }
    }

    public override void thisFixedUpdate()
    {
        base.thisFixedUpdate();

        if (phase == AttackPhase.Lunge)
        {
            MoveToward(plant.lungeTarget, plant.lungeSpeed);

            if (Vector2.Distance(plant.rb.position, plant.lungeTarget) <= 0.05f)
            {
                phase = AttackPhase.Recede;
            }
        }
        else if (phase == AttackPhase.Recede)
        {
            MoveToward(plant.startPosition, plant.recedeSpeed);

            if (Vector2.Distance(plant.rb.position, plant.startPosition) <= plant.returnStopDistance)
            {
                plant.rb.MovePosition(plant.startPosition);
                plant.rb.linearVelocity = Vector2.zero;

                phase = AttackPhase.Cooldown;
                timer = plant.cooldownDuration;
            }
        }
    }

    private void MoveToward(Vector2 target, float speed)
    {
        Vector2 newPos = Vector2.MoveTowards(
            plant.rb.position,
            target,
            speed * Time.fixedDeltaTime
        );

        plant.rb.MovePosition(newPos);
    }
}
