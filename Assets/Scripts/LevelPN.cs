using System.Collections.Generic;
using UnityEngine;

public class LevelPN : MonoBehaviour
{
    [SerializeField] private PoolPathNods _poolPathNods;
    [SerializeField] private Vector2Int _sizeGrid;
    [SerializeField] private CameraRay _cameraRay;

    private Vector3 _startPosition;
    private Vector3 _finishPosition;
    private GridPN _grid;
    private PathFinding _pathFinding;
    private List<PathNode> _path;

    private void Awake()
    {
        _grid = new GridPN(_sizeGrid, _poolPathNods);
        _pathFinding = new PathFinding(_grid);
    }

    private void Start() =>
        _grid.Create();

    private void Update()
    {
        SetFinishPosition();
        SetStartPosition();
        SetNotWalkingNode();
        Reset();
        FindPath();
    }

    private bool SetFinishPosition()
    {
        if (Input.GetMouseButtonDown(1))
        {
            PathNode finishNode = _cameraRay.GetNode();

            if (finishNode == null)
                return true;

            _finishPosition = finishNode.Position;
            SetColorElement(finishNode, Color.red);
        }

        return false;
    }

    private void SetStartPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PathNode startNode = _cameraRay.GetNode();

            if (startNode == null)
                return;

            _startPosition = startNode.Position;
            SetColorElement(startNode, Color.green);
        }
    }

    private void FindPath()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _path = _pathFinding.FindPath(_startPosition, _finishPosition);

            if (_path == null)
                return;

            for (int i = 1; i < _path.Count - 1; i++) 
                SetColorElement(_path[i], Color.yellow);
        }
    }

    private void Reset()
    {
        if (Input.GetKey(KeyCode.R))
        {
            foreach (var node in _path) 
                node.SetColor(node.DefaultColorNode);

            _startPosition = Vector3.zero;
            _finishPosition = Vector3.zero;
            
            _path.Clear();
        }
    }

    private void SetNotWalkingNode()
    {
        if (Input.GetMouseButtonDown(2))
        {
            PathNode pathNode = _cameraRay.GetNode();
            pathNode.SetImpassable();
            SetColorElement(pathNode, Color.black);
        }
    }

    private void SetColorElement(Component node, Color color) =>
        node.GetComponent<Renderer>().material.color = color;
}