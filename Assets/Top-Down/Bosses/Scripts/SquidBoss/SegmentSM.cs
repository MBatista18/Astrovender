using UnityEngine;
using System.Collections;

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

            for (int i = 0; i < armManager.armSegments.Length; i++)
            {
                armManager.armSegments[i].BeginFlashing();
            }

            armManager?.ChangeState(armManager.stateRetract);
        }

        //base.TakeDamage(damageAmount, attackerPos);
    }

    SpriteRenderer spriteRenderer;

    public override void InstantiateComponents()
    {
        base.InstantiateComponents();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    Coroutine flashingCoroutine;

    public void BeginFlashing()
    {
        if (flashingCoroutine != null)
        {
            StopCoroutine(flashingCoroutine);
        }
        flashingCoroutine = StartCoroutine(flash());
    }

    IEnumerator flash()
    {
        int i = 0;
        while (i < 3)
        {
            i++;

            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(.1f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(.1f);
        }
        
    }
}
