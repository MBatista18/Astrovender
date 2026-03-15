using UnityEngine;

public class EnemyShield : MonoBehaviour
{
    EnemySM sm;

    SpriteRenderer spriteRenderer;
    AstrovenderStructs.facingDirection facingDirection = AstrovenderStructs.facingDirection.down;
    public void SetFacingDirection(AstrovenderStructs.facingDirection newFacingDirection) { facingDirection = newFacingDirection; }

    [SerializeField] Sprite verticalShield;
    [SerializeField] Sprite horizontalShield;

    private void Awake()
    {
        sm = GetComponentInParent<EnemySM>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    [SerializeField] float offset = .25f;
    [SerializeField] float scale = 1;

    void Update()
    {
        spriteRenderer.sprite = verticalShield;
        spriteRenderer.sortingOrder = -1;
        transform.localScale = Vector3.one * scale;

        switch (sm.GetFacingDirection())
        {
            case AstrovenderStructs.facingDirection.down:
                spriteRenderer.sortingOrder = 1;
                transform.localPosition = new Vector3(0, -offset);
                break;
            case AstrovenderStructs.facingDirection.up:
                transform.localPosition = new Vector3(0, offset);
                break;
            case AstrovenderStructs.facingDirection.left:
                spriteRenderer.sprite = horizontalShield;
                transform.localPosition = new Vector3(-offset, 0);
                transform.localScale = new Vector3(-scale, scale, scale);
                break;
            case AstrovenderStructs.facingDirection.right:
                spriteRenderer.sprite = horizontalShield;
                transform.localPosition = new Vector3(offset, 0);
                break;
        }
    }
}
