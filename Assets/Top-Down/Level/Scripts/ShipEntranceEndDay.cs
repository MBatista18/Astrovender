using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipEntranceEndDay : MonoBehaviour
{
    [SerializeField] int endOfDayScene_ID = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance == null) { Debug.LogError("No Game Manager present, day will not end"); return; }

        GameManager.Instance.Progress(true);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            SceneManager.LoadScene(endOfDayScene_ID);
        }
    }

    [SerializeField] Sprite spriteClosed;
    [SerializeField] Sprite spriteOpen;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, AssetCall.instance.playerSM.transform.position) < 4)
        {
            spriteRenderer.sprite = spriteOpen;
        }
        else
        {
            spriteRenderer.sprite = spriteClosed;
        }
    }
}
