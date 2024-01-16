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
    private List<CellView> _path;

    private void Awake()
    {
        _grid = new GridPN(_sizeGrid, poolPathCell);
        _pathFinding = new PathFinding(_grid);
    }

    private void Start() =>
        _grid.Create();

    private void SetFinishPosition()
    {
        if (Input.GetMouseButtonUp(0))
        {
            CellView startNode = _cameraRay.GetNode();

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
            CellView startNode = _cameraRay.GetNode();

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

    public List<CellView> FindPath(Vector3 startPosition, Vector3 finishPosition)
    {
        return _pathFinding.FindPath(startPosition, finishPosition);
    }

    public void SetNotWalkingNode()
    {
        CellView cellView = _cameraRay.GetNode();
        _grid.Cells[cellView].SetImpassable();
        SetColorElement(cellView, Color.black);
    }

    private void SetColorElement(CellView node, Color color) =>
        node.SetColor(color);
}