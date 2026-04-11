using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStateDead : StateBase
{
    PlayerStateMachine sm;

    public PlayerStateDead(StateMachineBase _sm) : base(_sm)
    {
        sm = (PlayerStateMachine) _sm;
    }

    float timer;

    public override void thisStart()
    {
        base.thisStart();

        sm.SetCanChangeStates(false);

        sm.GetAnimationController().SetAnimatorState(PlayerAnimationController.AnimatorState.Death);
        sm.GetAnimationController().Animate();

        sm.GetCollider2D().enabled = false;
        
        AssetCall.instance.HUDText.BeginCountdown();

        timer = 0;
    }

    public override void thisUpdate()
    {
        base.thisUpdate();
        timer += Time.deltaTime;

        if (timer > 3.2f)
        {
            GameManager.Instance.Progress(false);
            SceneManager.LoadScene(1);
        }
    }

    public override void thisEnd()
    {
        base.thisEnd();

        // do something here;
    }
}
