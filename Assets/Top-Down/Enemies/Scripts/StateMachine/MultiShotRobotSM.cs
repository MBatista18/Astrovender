using UnityEngine;

public class MultiShotRobotSM : EnemySM
{
    
    [SerializeField] GameObject bullet;
    public GameObject GetBullet() { return bullet; }

    [SerializeField] Sprite cardinalSprite;
    [SerializeField] Sprite ordinalSprite;

    SpriteRenderer spriteRenderer;
    public void SwapRenderer(bool cardinalDirections) { spriteRenderer.sprite = cardinalDirections ? cardinalSprite : ordinalSprite; }

    MultiShotRobotStateFire stateFire;
    public override StateBase InitialState()
    {
        return stateFire;
    }
    public override void InstantiateStates()
    {
        stateFire = new MultiShotRobotStateFire(this);
    }

    public override void InstantiateValues()
    {
        base.InstantiateValues();
        base.SetReactToDamage(false);
    }

    public override void InstantiateComponents()
    {
        base.InstantiateComponents();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
