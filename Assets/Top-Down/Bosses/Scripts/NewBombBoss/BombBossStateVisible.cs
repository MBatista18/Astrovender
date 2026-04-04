using UnityEngine;

public class BombBossStateVisible : StateBase
{
    // The boss pops up from a port and shoots an energy ball at the player
    // After a short time, it goes back into hiding
    // The boss is vulnerable to attacks
    BombBossSM sm;
    public BombBossStateVisible(StateMachineBase _sm) : base(_sm)
    {
        sm = (BombBossSM)_sm;
    }
}
