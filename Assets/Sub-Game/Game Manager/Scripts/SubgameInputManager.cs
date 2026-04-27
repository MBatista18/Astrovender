using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Handles selecting / swapping tiles
[RequireComponent(typeof(SubgameBoard), typeof(PlayerInput))]
public class SubgameInputManager : MonoBehaviour
{
    [SerializeField] bool showDebugLogs = false;

    private PlayerInput _playerInput;
    private InputAction _touchPositionAction;
    private InputAction _touchPressAction;

    private SubgameBoard _board;
    private Camera _mainCam;
    private Node _selectedNode;

    // keep track of highlighted nodes so we can clear them reliably
    private readonly List<Node> _highlightedNodes = new List<Node>();

    // tracking pointer state for continuous follow
    private bool _isPointerDown;

    // original cell coordinates of selected node (do not change grid X/Y while dragging)
    private int _startX;
    private int _startY;

    // current candidate target node under pointer (highlighted)
    private Node _currentTarget;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _touchPressAction = _playerInput.actions["TouchPress"];
        _touchPositionAction = _playerInput.actions["TouchPosition"];

        _board = GetComponent<SubgameBoard>();
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

    // Called when a touch press is performed or canceled, then stores the screen position
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
            OnPointerUp();
        }
    }

    // On touch press, selects the node at the touch position and begins tracking it for dragging. Highlights the selected node.
    private void OnPointerDown(Vector2 screenPos)
    {
        if (_board.BoardBusy) return;

        Vector2 world = _mainCam.ScreenToWorldPoint(screenPos);
        var node = _board.GetNodeAtWorld(world);
        if (node == null) return;

        if (showDebugLogs) Debug.Log($"Selected node {node.name} at ({world.x}, {world.y})");

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

    // On touch release, if we have a valid target, attempt swap. If target is invalid or missing, return selected node to original position. Clear all highlights and reset state.
    private void OnPointerUp()
    {
        if (_selectedNode == null) return;

        // if we have a valid target outstanding, attempt swap
        if (_currentTarget != null && !_board.BoardBusy)
        {
            // ensure target is still present in the board
            if (_board.GetNodeAt(_currentTarget.X, _currentTarget.Y) == _currentTarget)
            {
                // verify adjacency (prevents diagonal swaps)
                int dx = Mathf.Abs(_currentTarget.X - _startX);
                int dy = Mathf.Abs(_currentTarget.Y - _startY);
                bool adjacent = (dx + dy) == 1;

                if (adjacent)
                {
                    _board.TrySwap(_selectedNode, _currentTarget);
                }
                else
                {
                    // non-adjacent target — return selected node to its origin
                    if (showDebugLogs) Debug.Log("Target not adjacent — returning selected node to origin.");
                    _selectedNode.StartCoroutine(_selectedNode.MoveToPosition(_board.GetCellCenter(_startX, _startY), 0.12f));
                }
            }
            else
            {
                // target disappeared or changed — return selected node to origin
                if (showDebugLogs) Debug.Log("Target invalid at release — returning selected node to origin.");
                _selectedNode.StartCoroutine(_selectedNode.MoveToPosition(_board.GetCellCenter(_startX, _startY), 0.12f));
            }
        }
        else
        {
            // no target selected or board is busy: return selected node to its original cell visually
            _selectedNode.StartCoroutine(_selectedNode.MoveToPosition(_board.GetCellCenter(_startX, _startY), 0.12f));
        }

        // clear visuals and state (selected node may still be animating back)
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

            // center of the start cell
            Vector3 startCenter = _board.GetCellCenter(_startX, _startY);

            // compute differences from start center
            float dx = world.x - startCenter.x;
            float dy = world.y - startCenter.y;

            // If within the start cell, keep the node at start center
            float snapThreshold = _board.CellSize * 0.25f;
            if (Mathf.Abs(dx) <= snapThreshold && Mathf.Abs(dy) <= snapThreshold)
            {
                _selectedNode.transform.position = startCenter;
                SetCurrentTarget(null);
                return;
            }

            // Decide dominant axis to prevent diagonal movement:
            // - If horizontal movement dominates, lock Y to start center and allow X to move only within one cell left/right.
            // - If vertical movement dominates, lock X to start center and allow Y to move only within one cell up/down.
            bool horizontal = Mathf.Abs(dx) > Mathf.Abs(dy);

            if (horizontal)
            {
                float minX = startCenter.x - _board.CellSize;
                float maxX = startCenter.x + _board.CellSize;
                // clamp to board world bounds
                minX = Mathf.Max(minX, _board.Origin.x);
                maxX = Mathf.Min(maxX, _board.Origin.x + (_board.Width - 1) * _board.CellSize);

                float xPos = Mathf.Clamp(world.x, minX, maxX);
                _selectedNode.transform.position = new Vector3(xPos, startCenter.y, _selectedNode.transform.position.z);

                // determine target cell along X axis
                int targetX = dx > 0 ? _startX + 1 : _startX - 1;
                int targetY = _startY;

                // only set target if within board bounds
                if (targetX >= 0 && targetX < _board.Width)
                {
                    // compute how far toward the neighbor the pointer is (use half-cell as activation)
                    if (Mathf.Abs(dx) >= _board.CellSize * 0.5f)
                    {
                        var node = _board.GetNodeAt(targetX, targetY);
                        SetCurrentTarget(node);
                    }
                    else
                    {
                        SetCurrentTarget(null);
                    }
                }
                else
                {
                    SetCurrentTarget(null);
                }
            }
            else // vertical movement
            {
                float minY = startCenter.y - _board.CellSize;
                float maxY = startCenter.y + _board.CellSize;
                // clamp to board world bounds
                minY = Mathf.Max(minY, _board.Origin.y);
                maxY = Mathf.Min(maxY, _board.Origin.y + (_board.Height - 1) * _board.CellSize);

                float yPos = Mathf.Clamp(world.y, minY, maxY);
                _selectedNode.transform.position = new Vector3(startCenter.x, yPos, _selectedNode.transform.position.z);

                // determine target cell along Y axis
                int targetX = _startX;
                int targetY = dy > 0 ? _startY + 1 : _startY - 1;

                // only set target if within board bounds
                if (targetY >= 0 && targetY < _board.Height)
                {
                    if (Mathf.Abs(dy) >= _board.CellSize * 0.5f)
                    {
                        var node = _board.GetNodeAt(targetX, targetY);
                        SetCurrentTarget(node);
                    }
                    else
                    {
                        SetCurrentTarget(null);
                    }
                }
                else
                {
                    SetCurrentTarget(null);
                }
            }
        }
    }

    // Updates the current target node and its visual state. Clears previous target highlight if changing.
    private void SetCurrentTarget(Node node)
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
        foreach (var node in _highlightedNodes)
        {
            if (node == null) continue;
            node.ResetHighlight();
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
