using System.Collections;
using UnityEngine;

public class LavaFlowObstacle : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    [Header("Visual")]
    [SerializeField] private Transform lavaVisual;
    [SerializeField] private SpriteRenderer lavaRenderer;

    [Header("Colliders")]
    [SerializeField] private BoxCollider2D damageTrigger;
    [SerializeField] private BoxCollider2D blockingCollider;

    [Header("Timing")]
    [SerializeField] private float startDelay = 0f;
    [SerializeField] private float flowDownDuration = 1f;
    [SerializeField] private float activeDuration = 3f;
    [SerializeField] private float cooldownDuration = 2f;
    [SerializeField] private float drainDuration = 0.75f;

    [Header("Damage")]
    [SerializeField] private int damageAmount = -1;
    [SerializeField] private float damageInterval = 0.5f;

    private bool isActive;
    private bool playerInside;
    private float damageTimer;
    private float lavaWidth;

    [SerializeField] Animator animator;

    AudioCall audioCall;

    private void Awake()
    {
        audioCall = GetComponent<AudioCall>();
    }

    private void Start()
    {
        lavaWidth = lavaRenderer.size.x;

        SetLavaByTopAndBottom(startPoint.position.y, startPoint.position.y);

        StartCoroutine(BeginAfterDelay());
    }

    private IEnumerator BeginAfterDelay()
    {
        yield return new WaitForSeconds(startDelay);
        yield return StartCoroutine(LavaCycle());
    }

    private void Update()
    {

        if (isActive && playerInside)
        {
            AssetCall.instance.playerSM.Knockback(Vector2.right * 2 * (AssetCall.instance.playerSM.transform.position.x < transform.position.x ? 1 : -1), 0.5f);
            PlayerHealth.ModifyOxygenLevel(damageAmount, false, transform.position);

            damageTimer -= Time.deltaTime;

            if (damageTimer <= 0f)
            {
                Debug.Log("Player took damage");
                damageTimer = damageInterval;
            }
        }
    }

    private IEnumerator LavaCycle()
    {
        float topY = startPoint.position.y;
        float bottomY = endPoint.position.y;

        while (true)
        {
            isActive = true;
            yield return StartCoroutine(FlowDown(topY, bottomY));

            yield return new WaitForSeconds(activeDuration);

            yield return StartCoroutine(DrainDown(topY, bottomY));

            isActive = false;
            playerInside = false;

            yield return new WaitForSeconds(cooldownDuration);
        }
    }

    private IEnumerator FlowDown(float topY, float bottomY)
    {
        playerInside = false;

        float elapsed = 0f;

        animator?.Play("LavaHoleBeginFlow");

        audioCall.CallAudioClip("Spill");

        while (elapsed < flowDownDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / flowDownDuration);

            float currentBottom = Mathf.Lerp(topY, bottomY, t);
            SetLavaByTopAndBottom(topY, currentBottom);

            yield return null;
        }

        SetLavaByTopAndBottom(topY, bottomY);
    }

    private IEnumerator DrainDown(float topY, float bottomY)
    {
        float elapsed = 0f;

        animator?.Play("LavaHoleEndFlow");

        while (elapsed < drainDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / drainDuration);

            float currentTop = Mathf.Lerp(topY, bottomY, t);
            SetLavaByTopAndBottom(currentTop, bottomY);

            yield return null;
        }

        SetLavaByTopAndBottom(bottomY, bottomY);
    }

    private void SetLavaByTopAndBottom(float topY, float bottomY)
    {
        float height = Mathf.Abs(topY - bottomY);
        float centerY = (topY + bottomY) * 0.5f;

        Vector3 visualPos = lavaVisual.position;
        visualPos.x = startPoint.position.x;
        visualPos.y = centerY;
        visualPos.z = lavaVisual.position.z;
        lavaVisual.position = visualPos;

        lavaRenderer.size = new Vector2(lavaWidth, height);

        UpdateWorldAlignedCollider(damageTrigger, centerY, height);
        UpdateWorldAlignedCollider(blockingCollider, centerY, height);
    }

    private void UpdateWorldAlignedCollider(BoxCollider2D col, float centerY, float height)
    {
        if (col == null) return;

        Transform t = col.transform;

        Vector3 pos = t.position;
        pos.x = startPoint.position.x;
        pos.y = centerY;
        t.position = pos;

        Vector2 size = col.size;
        size.y = height;
        col.size = size;

        col.offset = new Vector2(col.offset.x, 0f);
    }

    //Upon entering the lava collision, the player takes tick damage
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Player inside");
            playerInside = true;
            damageTimer = 0f;
        }
    }

    //Upon leaving the lava collision, the boolean that tracks the player is updated
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Player outside");
            playerInside = false;
        }
    }
}