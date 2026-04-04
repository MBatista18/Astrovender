using UnityEngine;

public class SegmentSM : EnemySM
{
    [SerializeField] ArmManager armManager;

    [SerializeField] bool isWeakPoint;
    public bool GetIsWeakPoint() { return isWeakPoint; }

    [HideInInspector] public bool CanRetractNow;

    public override void TakeDamage(int damageAmount, Vector3 attackerPos)
    {
        Debug.Log("Respond");

        if (CanRetractNow && isWeakPoint)
        {
            if (armManager.GetCurrentState() == armManager.stateMove || armManager.GetCurrentState() == armManager.stateRetract)
            {
                return;
            }

            armManager?.ChangeState(armManager.stateRetract);
        }

        //base.TakeDamage(damageAmount, attackerPos);
    }
}
