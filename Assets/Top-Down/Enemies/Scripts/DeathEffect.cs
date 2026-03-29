using UnityEngine;
using System.Collections;

public class DeathEffect : MonoBehaviour
{
    [Header("Sorting")]
    [SerializeField] private int sortingOrder = 10;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite firstSprite;
    [SerializeField] private Sprite secondSprite;
    [SerializeField] private float frameTime = 0.1f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Apply sorting immediately
        spriteRenderer.sortingOrder = sortingOrder;
    }

    private void Start()
    {
        StartCoroutine(PlayDeathEffect());
    }

    private IEnumerator PlayDeathEffect()
    {
        // Show first sprite
        spriteRenderer.sprite = firstSprite;
        yield return new WaitForSeconds(frameTime);

        // Show second sprite
        spriteRenderer.sprite = secondSprite;
        yield return new WaitForSeconds(frameTime);

        // Remove the effect object
        Destroy(gameObject);
    }
}
