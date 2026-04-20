using System.Collections;
using UnityEngine;

// Enum representing different node types in the subgame. Primarily used for visual differentiation, but may also be used for type-based logic
public enum NodeType
{
    Oxygen = 0,
    Bombs = 1,
    Ammo = 2,
    Shield = 3, 
    Coins = 4
}

// Represents a single cell in the subgame grid. Handles visuals and highlighting states.
[RequireComponent(typeof(SpriteRenderer))]
public class Node : MonoBehaviour
{
    public NodeType Type { get; private set; }
    public int X { get; set; }
    public int Y { get; set; }

    SpriteRenderer sr;

    [Header("Highlight Visuals")]
    [SerializeField] Color normalColor = Color.white;
    [SerializeField] Color selectedColor = new(1f, 0.85f, 0.25f);
    [SerializeField] Color adjacentColor = new(0.85f, 0.85f, 0.85f);
    [SerializeField] float normalScale = 1f;
    [SerializeField] float selectedScale = 1.12f;

    bool isSelected;
    bool isAdjacent;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        ApplyVisuals();
    }

    public void Init(NodeType type, int x, int y, Sprite sprite = null)
    {
        Type = type;
        X = x;
        Y = y;
        if (sprite != null) sr.sprite = sprite;
        ApplyVisuals();
    }

    public void SetSprite(Sprite sprite)
    {
        if (sr != null) sr.sprite = sprite;
    }

    public IEnumerator MoveToPosition(Vector3 target, float duration)
    {
        Vector3 start = transform.position;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(start, target, Mathf.SmoothStep(0f, 1f, t / duration));
            yield return null;
        }
        transform.position = target;
    }

    // Highlight API used by input manager
    public void SetSelected(bool selected)
    {
        isSelected = selected;
        if (selected)
        {
            // when selected, not considered adjacent
            isAdjacent = false;
        }
        ApplyVisuals();
    }

    public void SetAdjacent(bool adjacent)
    {
        // do not override selected
        if (isSelected) return;
        isAdjacent = adjacent;
        ApplyVisuals();
    }

    public void ResetHighlight()
    {
        isSelected = false;
        isAdjacent = false;
        ApplyVisuals();
    }

    void ApplyVisuals()
    {
        if (sr == null) return;

        if (isSelected)
        {
            sr.color = selectedColor;
            transform.localScale = Vector3.one * selectedScale;
        }
        else if (isAdjacent)
        {
            sr.color = adjacentColor;
            transform.localScale = Vector3.one * normalScale;
        }
        else
        {
            sr.color = normalColor;
            transform.localScale = Vector3.one * normalScale;
        }
    }
}
