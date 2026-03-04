using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Central board / match logic
public class Match3Board : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] int width = 6;
    [SerializeField] int height = 6;
    [SerializeField] float cellSize = 1.0f;
    [SerializeField] Vector2 origin = Vector2.zero; // world position of (0,0) cell (bottom-left)

    [Header("Prefabs & Visuals")]
    [Tooltip("Provide exactly 4 prefabs for NodeType order: TypeA, TypeB, TypeC, TypeD")]
    [SerializeField] GameObject[] nodePrefabs = new GameObject[4];
    [SerializeField] GameObject backgroundTilePrefab;
    [Tooltip("Optional parent for spawned nodes. If null, will use this GameObject's transform.")]
    [SerializeField] Transform nodeParent;

    [Header("Animation")]
    [SerializeField] float swapDuration = 0.12f;
    [SerializeField] float fallDuration = 0.12f;

    Match3Node[,] grid;

    bool boardBusy;

    public int Width => width;
    public int Height => height;
    public float CellSize => cellSize;
    public Vector2 Origin => origin;

    private void Awake()
    {
        grid = new Match3Node[width, height];
    }

    private void Start()
    {
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        StopAllCoroutines();
        boardBusy = false;

        // clear existing children
        if (nodeParent == null) nodeParent = this.transform;
        foreach (Transform t in nodeParent) { Destroy(t.gameObject); }

        grid = new Match3Node[width, height];
        FillBoardInitial();
    }

    private void FillBoardInitial()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject backgroundTile = Instantiate(backgroundTilePrefab, CellToWorld(x, y), Quaternion.identity, nodeParent);
                backgroundTile.name = $"Background ({x}, {y})";
                SpawnNodeAt(x, y, RandomNodeType());
            }
        }

        // Optional: remove any starting matches by re-rolling those positions
        var initialMatches = FindAllMatches();
        while (initialMatches.Count > 0)
        {
            foreach (var n in initialMatches)
            {
                Destroy(n.gameObject);
                SpawnNodeAt(n.X, n.Y, RandomNodeType());
            }
            initialMatches = FindAllMatches();
        }
    }

    NodeType RandomNodeType()
    {
        int r = Random.Range(0, 4);
        return (NodeType)r;
    }

    void SpawnNodeAt(int x, int y, NodeType type)
    {
        GameObject prefab = nodePrefabs[(int)type];
        if (backgroundTilePrefab == null && prefab == null)
        {
            Debug.LogError($"No prefab assigned for NodeType {type}. Please assign in inspector.");
            return;
        }

        var obj = Instantiate(prefab, CellToWorld(x, y), Quaternion.identity, nodeParent);
        obj.name = $"{type} Node ({x}, {y})";
        if (!obj.TryGetComponent<Match3Node>(out var nodeComp)) nodeComp = obj.AddComponent<Match3Node>();
        nodeComp.Init(type, x, y);

        // if prefab has SpriteRenderer and you want to set sprite, handle in prefab
        grid[x, y] = nodeComp;
    }

    Vector3 CellToWorld(int x, int y)
    {
        return new Vector3(origin.x + x * cellSize, origin.y + y * cellSize, 0f);
    }

    public Match3Node GetNodeAt(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return null;
        return grid[x, y];
    }

    public Match3Node GetNodeAtWorld(Vector2 worldPos)
    {
        Vector2 local = worldPos - origin;
        int x = Mathf.FloorToInt(local.x / cellSize);
        int y = Mathf.FloorToInt(local.y / cellSize);
        return GetNodeAt(x, y);
    }

    // Public call to attempt a swap initiated by input code
    public void TrySwap(Match3Node a, Match3Node b)
    {
        if (boardBusy) return;
        if (a == null || b == null) return;
        if (!AreAdjacent(a, b)) return;

        StartCoroutine(SwapAndResolve(a, b));
    }

    bool AreAdjacent(Match3Node a, Match3Node b)
    {
        int dx = Mathf.Abs(a.X - b.X);
        int dy = Mathf.Abs(a.Y - b.Y);
        return (dx + dy) == 1;
    }

    IEnumerator SwapAndResolve(Match3Node a, Match3Node b)
    {
        boardBusy = true;

        // swap in grid
        SwapGridNodes(a, b);

        // animate swap
        Vector3 posA = CellToWorld(a.X, a.Y);
        Vector3 posB = CellToWorld(b.X, b.Y);
        var ca = a.StartCoroutine(a.MoveToPosition(posA, swapDuration));
        var cb = b.StartCoroutine(b.MoveToPosition(posB, swapDuration));
        yield return new WaitForSeconds(swapDuration + 0.01f);

        var matches = FindAllMatches();
        if (matches.Count == 0)
        {
            // swap back
            SwapGridNodes(a, b);
            posA = CellToWorld(a.X, a.Y);
            posB = CellToWorld(b.X, b.Y);
            a.StartCoroutine(a.MoveToPosition(posA, swapDuration));
            b.StartCoroutine(b.MoveToPosition(posB, swapDuration));
            yield return new WaitForSeconds(swapDuration + 0.01f);
            boardBusy = false;
            yield break;
        }

        // resolve matches loop (cascades)
        while (true)
        {
            var toRemove = matches;
            int removedCount = toRemove.Count;

            // Award resources (example: coins per tile removed)
            if (GameManager.Instance != null)
            {
                GameManager.Instance.IncrementCoins(removedCount);
            }

            // remove nodes
            foreach (var n in toRemove)
            {
                grid[n.X, n.Y] = null;
                Destroy(n.gameObject);
            }

            // collapse & refill
            yield return StartCoroutine(CollapseColumns());

            matches = FindAllMatches();
            if (matches.Count == 0) break;
        }

        boardBusy = false;
    }

    void SwapGridNodes(Match3Node a, Match3Node b)
    {
        // swap positions in grid and update X/Y
        int ax = a.X, ay = a.Y;
        int bx = b.X, by = b.Y;

        grid[ax, ay] = b;
        grid[bx, by] = a;

        a.X = bx; a.Y = by;
        b.X = ax; b.Y = ay;
    }

    HashSet<Match3Node> FindAllMatches()
    {
        HashSet<Match3Node> matches = new HashSet<Match3Node>();

        // horizontal runs
        for (int y = 0; y < height; y++)
        {
            int runStart = 0;
            for (int x = 0; x < width; x++)
            {
                if (x == runStart) continue;
                var a = grid[x, y];
                var b = grid[x - 1, y];
                if (a == null || b == null || a.Type != b.Type)
                {
                    int runLength = x - runStart;
                    if (runLength >= 3)
                    {
                        for (int rx = runStart; rx < x; rx++)
                        {
                            if (grid[rx, y] != null) matches.Add(grid[rx, y]);
                        }
                    }
                    runStart = x;
                }
            }
            // handle end of row
            int finalRunLength = width - runStart;
            if (finalRunLength >= 3)
            {
                for (int rx = runStart; rx < width; rx++)
                {
                    if (grid[rx, y] != null) matches.Add(grid[rx, y]);
                }
            }
        }

        // vertical runs
        for (int x = 0; x < width; x++)
        {
            int runStart = 0;
            for (int y = 0; y < height; y++)
            {
                if (y == runStart) continue;
                var a = grid[x, y];
                var b = grid[x, y - 1];
                if (a == null || b == null || a.Type != b.Type)
                {
                    int runLength = y - runStart;
                    if (runLength >= 3)
                    {
                        for (int ry = runStart; ry < y; ry++)
                        {
                            if (grid[x, ry] != null) matches.Add(grid[x, ry]);
                        }
                    }
                    runStart = y;
                }
            }
            int finalRunLength = height - runStart;
            if (finalRunLength >= 3)
            {
                for (int ry = runStart; ry < height; ry++)
                {
                    if (grid[x, ry] != null) matches.Add(grid[x, ry]);
                }
            }
        }

        return matches;
    }

    IEnumerator CollapseColumns()
    {
        // For each column, move non-null nodes down to fill nulls
        for (int x = 0; x < width; x++)
        {
            int writeY = 0;
            for (int readY = 0; readY < height; readY++)
            {
                var node = grid[x, readY];
                if (node != null)
                {
                    if (readY != writeY)
                    {
                        grid[x, writeY] = node;
                        node.Y = writeY;
                        grid[x, readY] = null;
                        // animate falling
                        node.StartCoroutine(node.MoveToPosition(CellToWorld(x, writeY), fallDuration));
                    }
                    writeY++;
                }
            }

            // fill remaining with new nodes at top (spawn above board then fall)
            for (int y = writeY; y < height; y++)
            {
                NodeType t = RandomNodeType();
                // spawn slightly above
                Vector3 spawnPos = CellToWorld(x, height + (y - writeY));
                GameObject prefab = nodePrefabs[(int)t];
                GameObject obj;
                if (prefab != null)
                {
                    obj = Instantiate(prefab, spawnPos, Quaternion.identity, nodeParent);
                }
                else
                {
                    obj = new GameObject($"Node_{x}_{y}_{t}");
                    obj.transform.position = spawnPos;
                    obj.transform.parent = nodeParent;
                    obj.AddComponent<SpriteRenderer>();
                }

                var nodeComp = obj.GetComponent<Match3Node>();
                if (nodeComp == null) nodeComp = obj.AddComponent<Match3Node>();
                nodeComp.Init(t, x, y);
                grid[x, y] = nodeComp;
                nodeComp.StartCoroutine(nodeComp.MoveToPosition(CellToWorld(x, y), fallDuration));
            }
        }

        // wait for all falling animations to complete
        yield return new WaitForSeconds(fallDuration + 0.02f);
    }

    // ---------- Helpers for SubgameInputManager logic ----------

    // Convert world position to cell coordinates. Returns false if the world pos is outside the board bounds.
    public bool WorldToCell(Vector2 worldPos, out int x, out int y)
    {
        Vector2 local = worldPos - origin;
        x = Mathf.FloorToInt(local.x / cellSize);
        y = Mathf.FloorToInt(local.y / cellSize);
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return false;
        }
        return true;
    }

    // Public access to cell center for moving visuals
    public Vector3 GetCellCenter(int x, int y)
    {
        return CellToWorld(x, y);
    }
}
