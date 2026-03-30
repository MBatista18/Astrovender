using UnityEngine;

public class Hat : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    bool hatFound;

    Sprite down;
    Sprite up;
    Sprite left;
    Sprite right;

    private void Start()
    {
        if (GameManager.Instance.currentdataObj.wornHat == "") { hatFound = false; return; }
        
        Cosmetic_ShopSO currentHat = Resources.Load(GameManager.Instance.currentdataObj.wornHat) as Cosmetic_ShopSO;

        if (currentHat == null) { hatFound = false; Debug.Log("No Hat found at: " + GameManager.Instance.currentdataObj.wornHat); }
        else 
        {
            hatFound = true;
            down = currentHat.Sprite; 
            up = currentHat.backFacingHat != null ? currentHat.backFacingHat : down; 
            left = currentHat.leftFacingHat != null ? currentHat.leftFacingHat : down;
            right = currentHat.rightFacingHat != null ? currentHat.rightFacingHat : down;
        }
    }

    private void Update()
    {
        if (!hatFound) { return; }
        UpdateSprite();
    }

    void UpdateSprite()
    {
        AstrovenderStructs.facingDirection playerDirection = AssetCall.instance.playerSM.GetFacingDirection();

        switch (playerDirection)
        {
            case AstrovenderStructs.facingDirection.up:
                spriteRenderer.sprite = up;
                break;
            case AstrovenderStructs.facingDirection.down:
                spriteRenderer.sprite = down;
                break;
            case AstrovenderStructs.facingDirection.left:
                spriteRenderer.sprite = left;
                break;
            case AstrovenderStructs.facingDirection.right:
                spriteRenderer.sprite = right;
                break;
        }
    }
}
