using UnityEngine;

public class BombBossStateVisible : StateBase
{
    // The boss pops up from a port and shoots an energy ball at the player
    // After a short time, it goes back into hiding
    // The boss is vulnerable to attacks
    BombBossSM sm;
    Transform player;

    public BombBossStateVisible(StateMachineBase _sm) : base(_sm)
    {
        sm = (BombBossSM)_sm;
        player = sm.Player;
    }

    float attackDelayTimer;
    float visibleTimer;
    bool hasAttacked;
    public override void thisStart()
    {
        base.thisStart();

        sm.SetVisibility(true);
        attackDelayTimer = Random.Range(0.5f, 1f);
        visibleTimer = Random.Range(2.5f, 3.25f);
        hasAttacked = false;

        sm.GetAnimator().Play("BrainPopOut");

        Debug.Log("Visible");
    }

    public override void thisUpdate()
    {
        base.thisUpdate();

        if (!hasAttacked)
        {
            attackDelayTimer -= Time.deltaTime;

            if (attackDelayTimer <= 0f)
            {
                GameObject energyBall = GameObject.Instantiate(sm.EnergyBall, sm.transform.position, Quaternion.identity);
                energyBall.GetComponent<EnergyBall>().Launch(player.position);
                hasAttacked = true;

                sm.GetAnimator().Play("BrainIdle");

            }
            else return; // Don't start visible timer until after the attack delay, so the boss doesn't hide immediately after shooting
        }

        visibleTimer -= Time.deltaTime;

        if (visibleTimer <= 0)
        {
            sm.ChangeState(sm.InitialState()); // Go back to hidden state
        }
    }
}
