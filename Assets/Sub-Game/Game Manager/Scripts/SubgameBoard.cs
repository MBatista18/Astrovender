using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Central board / match logic
public class SubgameBoard : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] int width = 5;
    [SerializeField] int height = 7;
    [SerializeField] float cellSize = 1.0f;
    [SerializeField] Vector2 origin = Vector2.zero; // world position of (0,0) cell (bottom-left)

    [Serializable]
    private struct NodeData
    {
        public NodeType nodeType;
        public GameObject prefab;
        public bool unlocked;
    }
    [Header("Node Types (Make sure each Node Type is only represented once)")]
    [SerializeField] List<NodeData> nodeDataList = new();

    [Header("Prefabs & Visuals")]
    [SerializeField] GameObject backgroundTilePrefab;
    [Tooltip("Optional parent for spawned nodes. If null, will use this GameObject's transform.")]
    [SerializeField] Transform nodeParent;

    [Header("Animation")]
    [SerializeField] float swapDuration = 0.12f;
    [SerializeField] float fallDuration = 0.12f;

    [Header("Gameplay")]
    [Tooltip("How many moves the player starts with")]
    [SerializeField] int startingMoves = 20;
    [SerializeField] bool startOnWakeup = true;

    private Node[,] _grid;
    private bool _boardBusy;
    private int _movesRemaining;

    // runtime unlocked state
    private List<NodeType> _availableTypes = new List<NodeType>();

    [Tooltip("Event that fires when the player's moves remaining changes. Passes the current moves remaining as an int.")]
    public UnityEvent<int> OnMovesChanged; // Event that passes current moves remaining
    [Tooltip("Event that fires when the player runs out of moves.")]
    public UnityEvent OnOutOfMoves;
    [Tooltip("Event that fires when a match is made and nodes are cleared. Provides the NodeType of the nodes cleared and how many were cleared.")]
    public UnityEvent<NodeType, int> OnNodesCleared;

    public int MovesRemaining => _movesRemaining;
    public int Width => width;
    public int Height => height;
    public float CellSize => cellSize;
    public Vector2 Origin => origin;

    private void Awake()
    {
        _grid = new Node[width, height];
        _movesRemaining = startingMoves;

        RefreshAvailableTypes();
    }

    private void Start()
    {
        if (startOnWakeup) InitializeBoard(width, height);
    }

    // Recalculate the list of available node types
    private void RefreshAvailableTypes()
    {
        if (nodeDataList == null || nodeDataList.Count == 0) return;

        _availableTypes.Clear();
        foreach (var nodeData in nodeDataList)
        {
            if (nodeData.unlocked)
            {
                _availableTypes.Add(nodeData.nodeType);
            }
        }

        // If nothing is available (all locked), default to at least one type to avoid runtime errors
        if (_availableTypes.Count == 0)
        {
            Debug.LogError("There are no Node Types unlocked, so defaulting to unlocking the first node type. Probably not intended");
            _availableTypes.Add((NodeType)0);
        }
    }

    // Public API to unlock/lock types at runtime
    public void UnlockNodeType(NodeType type)
    {
        int index = nodeDataList.FindIndex(node => node.nodeType == type);
        if (index != -1)
        {
            // Set NodeData to unlocked
            var nodeData = nodeDataList[index];
            nodeData.unlocked = true;
            nodeDataList[index] = nodeData;
        }
        RefreshAvailableTypes();
    }

    public void LockNodeType(NodeType type)
    {
        int index = nodeDataList.FindIndex(node => node.nodeType == type);
        if (index != -1)
        {
            // Set NodeData to locked
            var nodeData = nodeDataList[index];
            nodeData.unlocked = false;
            nodeDataList[index] = nodeData;
        }
        RefreshAvailableTypes();
    }

    public IReadOnlyList<NodeType> GetAvailableNodeTypes()
    {
        return _availableTypes.AsReadOnly();
    }

    public void InitializeBoard(int width, int height, bool offsetOrigin = false)
    {
        StopAllCoroutines();
        _boardBusy = false;

        // clear existing children
        if (nodeParent == null) nodeParent = this.transform;
        foreach (Transform t in nodeParent) { Destroy(t.gameObject); }

        _grid = new Node[width, height];
        if (offsetOrigin)
        {
            // Move the origin so that the grid's center is still generally aligned with the original origin
            int widthOffset = this.width - width;
            int heightOffset = this.height - height;
            Vector2 newOrigin = new Vector2(widthOffset * cellSize * 0.5f, heightOffset * cellSize * 0.5f);
            origin = newOrigin;
        }
        FillBoardInitial();

        OnMovesChanged?.Invoke(_movesRemaining);
    }

    private void FillBoardInitial()
    {
        // Spawn background tiles and initial nodes
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject backgroundTile = Instantiate(backgroundTilePrefab, CellToWorld(x, y), Quaternion.identity, nodeParent);
                backgroundTile.name = $"Background ({x}, {y})";
                SpawnNodeAt(x, y, RandomNodeType());
            }
        }

        // Remove any starting matches by re-rolling those positions
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

    // Get a random NodeType from the currently available/unlocked set
    private NodeType RandomNodeType()
    {
        if (_availableTypes == null || _availableTypes.Count == 0)
        {
            Debug.LogError("Available Types is null or empty, defaulting to a random node type (This shouldn't happen)");
            int r = UnityEngine.Random.Range(0, nodeDataList.Count);
            return (NodeType)r;
        }

        int idx = UnityEngine.Random.Range(0, _availableTypes.Count);
        return _availableTypes[idx];
    }

    // Spawn a node of the given type at the given cell coordinates.
    private void SpawnNodeAt(int x, int y, NodeType type)
    {
        NodeData nodeData = nodeDataList.Find(node => node.nodeType == type);
        GameObject prefab = nodeData.prefab;
        if (backgroundTilePrefab == null && prefab == null)
        {
            Debug.LogError($"No prefab assigned for NodeType {type}. Please assign in inspector.");
            return;
        }

        var obj = Instantiate(prefab, CellToWorld(x, y), Quaternion.identity, nodeParent);
        obj.name = $"{type} Node ({x}, {y})";
        if (!obj.TryGetComponent<Node>(out var nodeComp)) nodeComp = obj.AddComponent<Node>();
        nodeComp.Init(type, x, y);

        // if prefab has SpriteRenderer and you want to set sprite, handle in prefab
        _grid[x, y] = nodeComp;
    }

    // Convert cell coordinates to world position
    private Vector3 CellToWorld(int x, int y)
    {
        return new Vector3(origin.x + x * cellSize, origin.y + y * cellSize, 0f);
    }

    // Return the node at the given cell coordinates, or null if out of bounds or empty.
    public Node GetNodeAt(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return null;
        return _grid[x, y];
    }

    // Convert world position to cell coordinates and return the node at that cell.
    public Node GetNodeAtWorld(Vector2 worldPos)
    {
        Vector2 local = worldPos - origin;
        int x = Mathf.RoundToInt(local.x / cellSize);
        int y = Mathf.RoundToInt(local.y / cellSize);
        return GetNodeAt(x, y);
    }

    // Attempt to swap two nodes. Validates adjacency and move availability, then performs swap and resolves matches.
    public void TrySwap(Node a, Node b)
    {
        if (_boardBusy) return;
        if (a == null || b == null) return;
        if (!AreAdjacent(a, b)) return;

        if (_movesRemaining <= 0)
        {
            Debug.Log("No moves remaining.");
            return;
        }

        ConsumeMove();
        StartCoroutine(SwapAndResolve(a, b));
    }

    // Decrement moves and fire relevant events. If moves reach 0, fire OnOutOfMoves.
    private void ConsumeMove()
    {
        _movesRemaining = Mathf.Max(0, _movesRemaining - 1);
        OnMovesChanged?.Invoke(_movesRemaining);
        if (_movesRemaining == 0) OnOutOfMoves?.Invoke();
    }

    // Check if two nodes are orthogonally adjacent
    private bool AreAdjacent(Node a, Node b)
    {
        int dx = Mathf.Abs(a.X - b.X);
        int dy = Mathf.Abs(a.Y - b.Y);
        return (dx + dy) == 1;
    }

    // Perform the swap animation and resolve matches, including cascades.
    private IEnumerator SwapAndResolve(Node a, Node b)
    {
        _boardBusy = true;

        // swap in grid
        SwapGridNodes(a, b);

        // animate swap
        Vector3 posA = CellToWorld(a.X, a.Y);
        Vector3 posB = CellToWorld(b.X, b.Y);
        a.StartCoroutine(a.MoveToPosition(posA, swapDuration));
        b.StartCoroutine(b.MoveToPosition(posB, swapDuration));
        yield return new WaitForSeconds(swapDuration + 0.01f);

        var matches = FindAllMatches(out var type);
        if (matches.Count == 0)
        {
            // No matches created.
            _boardBusy = false;
            yield break;
        }

        // resolve matches
        while (matches.Count > 0)
        {
            var toRemove = matches;
            int removedCount = toRemove.Count;

            // Notify subscribers about cleared nodes
            OnNodesCleared?.Invoke(type, removedCount);

            // remove nodes
            foreach (var node in toRemove)
            {
                _grid[node.X, node.Y] = null;
                Destroy(node.gameObject);
            }

            // collapse & refill
            yield return StartCoroutine(CollapseColumns());

            matches = FindAllMatches();
        }

        _boardBusy = false;
    }

    private void SwapGridNodes(Node a, Node b)
    {
        // swap positions in grid and update X/Y
        int ax = a.X, ay = a.Y;
        int bx = b.X, by = b.Y;

        _grid[ax, ay] = b;
        _grid[bx, by] = a;

        a.X = bx; a.Y = by;
        b.X = ax; b.Y = ay;
    }

    // Find all nodes that are part of a horizontal or vertical run of 3 or more matching types. Returns a set of nodes to be cleared.
    // Has an overload that also outputs the type of the matched nodes
    private HashSet<Node> FindAllMatches()
    {
        HashSet<Node> matches = new HashSet<Node>();

        // horizontal runs
        for (int y = 0; y < height; y++)
        {
            int runStart = 0;
            for (int x = 0; x < width; x++)
            {
                if (x == runStart) continue;
                var a = _grid[x, y];
                var b = _grid[x - 1, y];
                if (a == null || b == null || a.Type != b.Type)
                {
                    int runLength = x - runStart;
                    if (runLength >= 3)
                    {
                        for (int rx = runStart; rx < x; rx++)
                        {
                            if (_grid[rx, y] != null) matches.Add(_grid[rx, y]);
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
                    if (_grid[rx, y] != null)
                    {
                        matches.Add(_grid[rx, y]);
                    }
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
                var a = _grid[x, y];
                var b = _grid[x, y - 1];
                if (a == null || b == null || a.Type != b.Type)
                {
                    int runLength = y - runStart;
                    if (runLength >= 3)
                    {
                        for (int ry = runStart; ry < y; ry++)
                        {
                            if (_grid[x, ry] != null) matches.Add(_grid[x, ry]);
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
                    if (_grid[x, ry] != null)
                    {
                        matches.Add(_grid[x, ry]);
                    }
                }
            }
        }

        return matches;
    }
    #region FindAllMatches Overload

    private HashSet<Node> FindAllMatches(out NodeType nodeType)
    {
        HashSet<Node> matches = new HashSet<Node>();
        nodeType = default;

        // horizontal runs
        for (int y = 0; y < height; y++)
        {
            int runStart = 0;
            for (int x = 0; x < width; x++)
            {
                if (x == runStart) continue;
                var a = _grid[x, y];
                var b = _grid[x - 1, y];
                if (a == null || b == null || a.Type != b.Type)
                {
                    int runLength = x - runStart;
                    if (runLength >= 3)
                    {
                        for (int rx = runStart; rx < x; rx++)
                        {
                            if (_grid[rx, y] != null) matches.Add(_grid[rx, y]);
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
                    if (_grid[rx, y] != null)
                    {
                        matches.Add(_grid[rx, y]);
                        nodeType = _grid[rx, y].Type; // set nodeType to the type of the matched nodes
                    }
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
                var a = _grid[x, y];
                var b = _grid[x, y - 1];
                if (a == null || b == null || a.Type != b.Type)
                {
                    int runLength = y - runStart;
                    if (runLength >= 3)
                    {
                        for (int ry = runStart; ry < y; ry++)
                        {
                            if (_grid[x, ry] != null) matches.Add(_grid[x, ry]);
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
                    if (_grid[x, ry] != null)
                    {
                        matches.Add(_grid[x, ry]);
                        nodeType = _grid[x, ry].Type; // set nodeType to the type of the matched nodes
                    }
                }
            }
        }

        return matches;
    }

    #endregion

    // After matches are cleared, collapse columns down and spawn new nodes at the top. Animates falling. Waits for animations to complete before returning.
    private IEnumerator CollapseColumns()
    {
        // For each column, move non-null nodes down to fill nulls
        for (int x = 0; x < width; x++)
        {
            int writeY = 0;
            for (int readY = 0; readY < height; readY++)
            {
                var node = _grid[x, readY];
                if (node != null)
                {
                    if (readY != writeY)
                    {
                        _grid[x, writeY] = node;
                        node.Y = writeY;
                        _grid[x, readY] = null;
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
                NodeData nodeData = nodeDataList.Find(node => node.nodeType == t);
                GameObject prefab = nodeData.prefab;
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

                var nodeComp = obj.GetComponent<Node>();
                if (nodeComp == null) nodeComp = obj.AddComponent<Node>();
                nodeComp.Init(t, x, y);
                _grid[x, y] = nodeComp;
                nodeComp.StartCoroutine(nodeComp.MoveToPosition(CellToWorld(x, y), fallDuration));
            }
        }

        // wait for all falling animations to complete
        yield return new WaitForSeconds(fallDuration + 0.02f);
    }

    public void SetStartOnWakeup(bool value)
    {
        startOnWakeup = value;
    }

    // ---------- Helpers for SubgameInputManager logic ----------

    // Convert world position to cell coordinates. Returns false if the world pos is outside the board bounds.
    public bool WorldToCell(Vector2 worldPos, out int x, out int y)
    {
        Vector2 local = worldPos - origin;
        x = Mathf.RoundToInt(local.x / cellSize);
        y = Mathf.RoundToInt(local.y / cellSize);
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
