using UnityEngine;

public class GalleryStateFly : StateBase
{
    GalleryTarget sm;

    public GalleryStateFly (StateMachineBase _sm) : base (_sm)
    {
        sm = (GalleryTarget)_sm;
    }

    public override void thisStart()
    {
        base.thisStart();

        if (sm.GetGalleryManager() == null) { return; }

        randomIndex = Random.Range(0, sm.GetGalleryManager().GetPositionsCount());
        iterationDirection = Random.Range(0, 10) < 4 ? -1 : 1;
        randomSpeedMultiplier = Random.Range(1f, 1.4f);
    }

    int randomIndex;

    float randomSpeedMultiplier;
    int iterationDirection;

    public override void thisUpdate()
    {
        base.thisUpdate();

        if (sm.GetGalleryManager() == null) { return; }

        if (Vector3.Distance(sm.transform.position, sm.GetGalleryManager().GetTransformAt(randomIndex).position) < 0.1f) { randomIndex += iterationDirection; }
        if (randomIndex >= sm.GetGalleryManager().GetPositionsCount()) { randomIndex = 0; }
        if (randomIndex < 0) { randomIndex = sm.GetGalleryManager().GetPositionsCount() - 1; }

        if (sm.GetGalleryManager().GetTransformAt(randomIndex).position.x < sm.transform.position.x)
        {
            sm.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            sm.transform.localScale = new Vector3(1, 1, 1);
        }

        sm.transform.position = Vector3.MoveTowards(sm.transform.position, sm.GetGalleryManager().GetTransformAt(randomIndex).position, 
            sm.GetMovementSpeed() * Time.deltaTime * randomSpeedMultiplier);
    }
}
