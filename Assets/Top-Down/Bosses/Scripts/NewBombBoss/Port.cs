using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class Port : MonoBehaviour
{
    private PortManager portManager;
    private bool activated = true;
    private bool hasBoss = false;
    private Coroutine reactivationCoroutine;

    private SpriteRenderer spriteRenderer;
    private Color tempActiveColor = Color.white;
    private Color tempInactiveColor = Color.gray;

    [SerializeField] Sprite closed, open, broken, semibroken;

    AudioSource audioSource;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        audioSource = GetComponent<AudioSource>();

        activated = false;
    }

    public void OnEnable()
    {
        if (reactivationCoroutine != null)
            StopCoroutine(reactivationCoroutine);
        reactivationCoroutine = null;
        
        GetComponent<Collider2D>().enabled = true;
        spriteRenderer.color = tempActiveColor; // Temp
    }

    public void OnDisable()
    {
        if (reactivationCoroutine != null)
            StopCoroutine(reactivationCoroutine);
        reactivationCoroutine = null;

        GetComponent<Collider2D>().enabled = false;
        spriteRenderer.color = tempInactiveColor; // Temp
    }

    public void Initialize(PortManager manager)
    {
        portManager = manager;
        reactivationCoroutine = null;

        ActivatePort();
    }

    public void ActivatePort()
    {
        activated = true;

        if (portManager != null) portManager.AddActivePort(this);

        // Additional logic for activating the port (e.g., visual effects)
        spriteRenderer.sprite = closed; // Temp
    }

    public void DeactivatePort()
    {
        if (activated)
        {
            activated = false;
            portManager?.RemoveActivePort(this);

            audioSource.Play();

            reactivationCoroutine ??= StartCoroutine(ReactivationTimer(5f));

            // Additional logic for deactivating the port (e.g., visual effects)
        }
    }

    public void OpenPort()
    {
        spriteRenderer.sprite = open;
    }

    public void ClosePort()
    {
        if (!activated) { return; }
        spriteRenderer.sprite = closed;
    }

    public void HideInPort()
    {
        portManager.SetBossPort(this);
    }

    public void SetBossPresence(bool presence)
    {
        hasBoss = presence;
    }

    public void OnExplosion()
    {
        if (reactivationCoroutine != null) return;

        DeactivatePort();
        portManager?.OnPortExplosion(this, hasBoss);
    }

    IEnumerator ReactivationTimer(float delay)
    {
        Debug.Log($"{gameObject.name} will reactivate in {delay} seconds.");

        spriteRenderer.sprite = broken;
        yield return new WaitForSeconds(delay/2);

        spriteRenderer.sprite = semibroken;
        yield return new WaitForSeconds(delay / 2);
        ActivatePort();
        reactivationCoroutine = null;
    }

}
