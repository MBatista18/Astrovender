using UnityEngine;

public class ShielderVar_DamagePlayerOnCollision : DamagePlayerOnCollision
{
    ShielderRobotSM sm;

    private void Awake()
    {
        sm = GetComponentInParent<ShielderRobotSM>();
    }

    public override void OnCollision()
    {
        sm.CallKnockback();
    }
}
