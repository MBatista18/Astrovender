using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    EnemySM sm;

    SpriteRenderer spriteRenderer;
    AstrovenderStructs.facingDirection facingDirection = AstrovenderStructs.facingDirection.down;
    public void SetFacingDirection(AstrovenderStructs.facingDirection newFacingDirection) { facingDirection = newFacingDirection; }

    [SerializeField] Sprite upshield,downshield,leftshield,rightshield;

    private void Awake()
    {
        sm = GetComponentInParent<EnemySM>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    [SerializeField] float offset = .25f;
    [SerializeField] float scale = 1;

    Animator animator;
    public void PlayHit()
    {
        animator.Play("ShieldHit", 0, 0);
    }

    void Update()
    {
        spriteRenderer.sprite = upshield;
        transform.localScale = Vector3.one * scale;

        switch (sm.GetFacingDirection())
        {
            case AstrovenderStructs.facingDirection.down:
                spriteRenderer.sprite = downshield;
                transform.localPosition = new Vector3(0, -offset);
                break;
            case AstrovenderStructs.facingDirection.up:
                transform.localPosition = new Vector3(0, offset);
                break;
            case AstrovenderStructs.facingDirection.left:
                spriteRenderer.sprite = leftshield;
                transform.localPosition = new Vector3(-offset, 0);
                break;
            case AstrovenderStructs.facingDirection.right:
                spriteRenderer.sprite = rightshield;
                transform.localPosition = new Vector3(offset, 0);
                break;
        }
    }
}
