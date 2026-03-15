using UnityEngine;

public class ShieldVisuals : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    [SerializeField] Sprite verticalShield;
    [SerializeField] Sprite horizontalShield;

    Animator animator;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void PlayHit()
    {
        animator.Play("ShieldHit", 0, 0);
    }

    void Update()
    {
        bool showSprite = true;

        if (PlayerManager.GetCurrentShieldHealth() <= 0) { showSprite = false; }
        if (!GameManager.Instance.collectedShield) { showSprite = false; }

        spriteRenderer.color = new Color(1, 1, 1, showSprite ? 1 : 0);
        animator.enabled = showSprite;

        if (!showSprite) { return; }

        spriteRenderer.sprite = verticalShield;
        spriteRenderer.sortingOrder = -1;
        transform.localScale = Vector3.one;

        switch (AssetCall.instance.playerSM.facingDirection)
        {
            case AstrovenderStructs.facingDirection.down:
                spriteRenderer.sortingOrder = 1;
                transform.localPosition = new Vector3(0, -.25f);
                break;
            case AstrovenderStructs.facingDirection.up:
                transform.localPosition = new Vector3(0, .25f);
                break;
            case AstrovenderStructs.facingDirection.left:
                spriteRenderer.sprite = horizontalShield;
                transform.localPosition = new Vector3(-.25f, 0);
                transform.localScale = new Vector3(-1,1,1);
                break;
            case AstrovenderStructs.facingDirection.right:
                spriteRenderer.sprite = horizontalShield;
                transform.localPosition = new Vector3(.25f, 0);
                break;
        }
    }
}
