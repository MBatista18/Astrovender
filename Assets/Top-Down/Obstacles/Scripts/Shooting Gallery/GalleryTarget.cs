using UnityEngine;

public class GalleryTarget : EnemySM
{
    GalleryStateFly stateFly;

    public void BeginFlying() { GetComponent<Animator>().Play("GalleryBirdFly"); ChangeState(stateFly); }

    public override void InstantiateStates()
    {
        base.InstantiateStates();
        transform.localScale = new Vector3(Random.Range(1, 10) < 5 ? -1 : 1, 1, 1);

        stateFly = new GalleryStateFly(this);
    }

    public override StateBase DeathState()
    {
        if (thisPrize != null) { thisPrize.SetToFall(); }
        //if (galleryManager != null) { galleryManager.CallBirds(); }

        return base.DeathState();
    }

    GalleryPrize thisPrize;
    GalleryManager galleryManager;
    public void SetGalleryManager(GalleryManager gm) { galleryManager = gm; }
    public GalleryManager GetGalleryManager() { return galleryManager; }


    public override void InstantiateComponents()
    {
        base.InstantiateComponents();
        thisPrize = GetComponentInChildren<GalleryPrize>();
    }
}
