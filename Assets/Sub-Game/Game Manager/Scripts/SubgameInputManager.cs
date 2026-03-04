using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Handles input for selecting / swapping tiles
[RequireComponent(typeof(Match3Board), typeof(PlayerInput))]
public class SubgameInputManager : MonoBehaviour
{
    [SerializeField] bool showDebugLogs = false;

    private PlayerInput _playerInput;
    private InputAction _touchPositionAction;
    private InputAction _touchPressAction;

    private Match3Board _board;
    private Camera _mainCam;
    private Match3Node _selectedNode;

    // keep track of highlighted nodes so we can clear them reliably
    private readonly List<Match3Node> _highlightedNodes = new List<Match3Node>();

    // tracking pointer state for continuous follow
    private bool _isPointerDown;

    // original cell coordinates of selected node (do not change grid X/Y while dragging)
    private int _startX;
    private int _startY;

    // current candidate target node under pointer (highlighted)
    private Match3Node _currentTarget;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _touchPressAction = _playerInput.actions["TouchPress"];
        _touchPositionAction = _playerInput.actions["TouchPosition"];

        _board = GetComponent<Match3Board>();
        _mainCam = Camera.main;
    }

    private void OnEnable()
    {
        _touchPressAction.performed += TouchPressed;
        _touchPressAction.canceled += TouchPressed;
    }

    private void OnDisable()
    {
        _touchPressAction.performed -= TouchPressed;
        _touchPressAction.canceled -= TouchPressed;

        ClearHighlights();
    }

    private void TouchPressed(InputAction.CallbackContext context)
    {
        Vector2 screenPos = _touchPositionAction.ReadValue<Vector2>();
        if (showDebugLogs) Debug.Log($"Touch position: {screenPos}, phase: {context.phase}");

        if (context.phase == InputActionPhase.Performed)
        {
            if (showDebugLogs) Debug.Log($"Touch started at {screenPos}");
            _isPointerDown = true;
            OnPointerDown(screenPos);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            if (showDebugLogs) Debug.Log($"Touch ended at {screenPos}");
            _isPointerDown = false;
            OnPointerUp(screenPos);
        }
    }

    private void OnPointerDown(Vector2 screenPos)
    {
        Vector2 world = _mainCam.ScreenToWorldPoint(screenPos);
        var node = _board.GetNodeAtWorld(world);
        if (node == null) return;

        // clear previous highlights if selecting a new node
        if (_selectedNode != node)
        {
            ClearHighlights();
        }

        _selectedNode = node;
        _startX = node.X;
        _startY = node.Y;

        _selectedNode.SetSelected(true);
        _highlightedNodes.Add(_selectedNode);

        // when starting to drag, we don't highlight the adjacent nodes automatically —
        // the target highlight will follow pointer instead
        _currentTarget = null;
    }

    private void OnPointerUp(Vector2 screenPos)
    {
        if (_selectedNode == null) return;

        // if we have a valid target outstanding, attempt swap
        if (_currentTarget != null)
        {
            // ensure target is adjacent in grid (should be, by clamp) and present
            if (_board.GetNodeAt(_currentTarget.X, _currentTarget.Y) == _currentTarget)
            {
                _board.TrySwap(_selectedNode, _currentTarget);
            }
        }
        else
        {
            // no target selected: return selected node to its original cell visually
            _selectedNode.StartCoroutine(_selectedNode.MoveToPosition(_board.GetCellCenter(_startX, _startY), 0.12f));
        }

        // clear visuals and state
        ClearHighlights();
        _selectedNode = null;
        _currentTarget = null;
    }
    private void Update()
    {
        // while pointer down and a node is selected, update the selected node's visual position to follow the pointer
        if (_isPointerDown && _selectedNode != null)
        {
            Vector2 screenPos = _touchPositionAction.ReadValue<Vector2>();
            Vector2 world = _mainCam.ScreenToWorldPoint(screenPos);

            // clamp world position to the 3x3 area centered on the start cell
            float halfCell = _board.CellSize * 0.5f;

            float minX = _board.Origin.x + (_startX - 1) * _board.CellSize - 0.001f;
            float maxX = _board.Origin.x + (_startX + 1) * _board.CellSize + 0.001f;
            float minY = _board.Origin.y + (_startY - 1) * _board.CellSize - 0.001f;
            float maxY = _board.Origin.y + (_startY + 1) * _board.CellSize + 0.001f;

            // clamp to board bounds as well
            minX = Mathf.Max(minX, _board.Origin.x);
            minY = Mathf.Max(minY, _board.Origin.y);
            maxX = Mathf.Min(maxX, _board.Origin.x + (_board.Width - 1) * _board.CellSize);
            maxY = Mathf.Min(maxY, _board.Origin.y + (_board.Height - 1) * _board.CellSize);

            Vector3 clamped = new Vector3(Mathf.Clamp(world.x, minX, maxX), Mathf.Clamp(world.y, minY, maxY), _selectedNode.transform.position.z);
            _selectedNode.transform.position = clamped;

            // determine which cell the pointer is over, then clamp that cell to the 3x3 allowed area
            if (_board.WorldToCell(world, out int cx, out int cy))
            {
                int tx = Mathf.Clamp(cx, _startX - 1, _startX + 1);
                int ty = Mathf.Clamp(cy, _startY - 1, _startY + 1);

                // don't highlight the original selected cell
                if (tx == _startX && ty == _startY)
                {
                    SetCurrentTarget(null);
                }
                else
                {
                    var node = _board.GetNodeAt(tx, ty);
                    SetCurrentTarget(node);
                }
            }
            else
            {
                // if outside board, clear target
                SetCurrentTarget(null);
            }
        }
    }


    private void SetCurrentTarget(Match3Node node)
    {
        if (_currentTarget == node) return;

        // clear previous target visual
        if (_currentTarget != null)
        {
            _currentTarget.SetAdjacent(false);
            _highlightedNodes.Remove(_currentTarget);
        }

        _currentTarget = node;

        if (_currentTarget != null)
        {
            _currentTarget.SetAdjacent(true); // reuse adjacent visual to indicate swap candidate
            if (!_highlightedNodes.Contains(_currentTarget)) _highlightedNodes.Add(_currentTarget);
        }
    }

    private void ClearHighlights()
    {
        // reset selected & adjacent states for tracked nodes
        foreach (var n in _highlightedNodes)
        {
            if (n == null) continue;
            n.ResetHighlight();
        }
        _highlightedNodes.Clear();

        // ensure selected node is also reset if it wasn't tracked
        if (_selectedNode != null)
        {
            _selectedNode.ResetHighlight();
        }

        _currentTarget = null;
    }
}
