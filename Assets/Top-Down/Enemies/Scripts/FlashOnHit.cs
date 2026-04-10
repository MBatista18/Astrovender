using UnityEngine;
using System.Collections;

public class FlashOnHit : MonoBehaviour
{

    [SerializeField] private SpriteRenderer spriteRenderer;
    private Color originalColor;

    [SerializeField] private float flashDuration = 0.1f;
    private Coroutine flashCoroutine;

    private void Awake()
    {
        if (spriteRenderer == null) { spriteRenderer = GetComponentInChildren<SpriteRenderer>(); }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    //Used to test flash
    /*private void Start()
    {
        FlashRed();
    }*/

    public void FlashRed()
    {
        //Debug.Log("Flashing Red");

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
        flashCoroutine = null;
    }
}