using System.Collections.Generic;
using UnityEngine;

public class LevelPN : MonoBehaviour
{
    [SerializeField] private PoolPathCell poolPathCell;
    [SerializeField] private Vector2Int _sizeGrid;
    [SerializeField] private CameraRay _cameraRay;

    private Vector3 _startPosition;
    private Vector3 _finishPosition;

    private IGrid _grid;
    private PathFinding _pathFinding;
    private List<Cell> _path;

    private void Awake()
    {
        _grid = new GridPN(_sizeGrid, poolPathCell);
        _pathFinding = new PathFinding(_grid);
    }

    private void Start() =>
        _grid.Create();

    private void Update()
    {
        SetFinishPosition();
        SetStartPosition();
        
        SetNotWalkingNode();
        FindPath();
    }

    private void SetFinishPosition()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Cell startNode = _cameraRay.GetNode();

            if (startNode == null)
                return;

            _finishPosition = startNode.Position;
            SetColorElement(startNode, Color.red);
        }
    }

    private void SetStartPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cell startNode = _cameraRay.GetNode();

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

    private void SetNotWalkingNode()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Cell cell = _cameraRay.GetNode();
            _grid.Cells[cell].SetImpassable();
            SetColorElement(cell, Color.black);
        }
    }

    private void SetColorElement(Cell node, Color color) =>
        node.SetColor(color);
}