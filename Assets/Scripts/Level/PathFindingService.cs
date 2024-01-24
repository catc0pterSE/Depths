using System.Collections.Generic;
using DefaultNamespace;
using Grid;
using Grid.Elements;
using Grid.Elements.Work.Cell;
using PathFindingSystem;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinder = PathFindingSystem.Pathfinder;

namespace Level
{
    public class PathFindingService : MonoBehaviour, IPathFindingService
    {
        [SerializeField] private Vector2Int _sizeGrid;
        [SerializeField] private CameraController _camera;
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileBase _tileBase;
        [SerializeField] private TileBase _tileBase2;


        private Vector3 _startPosition;
        private Vector3 _finishPosition;

        private IGrid _grid;
        private IPathFinding _pathFinding;
        public IPathFinding _finder;
        private List<Vector3> _path;

        public IGrid Grid => _grid;
        private void Awake()
        {
            _grid = new GridPN(_sizeGrid, _tilemap, _tileBase);
            _pathFinding = new PathFinding(_grid);
            _finder = Object.FindObjectOfType<Pathfinder>();
        }

        private void Start() =>
            _grid.Create();

        public List<Vector3> FindPath(Vector3 startPosition, Vector3 finishPosition)
        {
            var start = new Vector3Int(Mathf.RoundToInt(startPosition.x), Mathf.RoundToInt(startPosition.y));
            var end = new Vector3Int(Mathf.RoundToInt(finishPosition.x), Mathf.RoundToInt(finishPosition.y));
            var path = _finder.FindPath(start, end);
            return path;
        }

        public void SetNotWalkingNode()
        {
            if (Input.GetMouseButtonDown(1))
            {
                CellPFModel cellView = _grid.FromWorldToCell(_camera.GetWorldPosition());
                cellView.SetImpassable();
                ChangeTile(cellView);
            }
        }

        private void ChangeTile(CellPFModel node)
        {
            _tilemap.GetTile(node.GridPosition.ToVector3());
            _tilemap.SetTile(node.GridPosition.ToVector3(), _tileBase2);
        }

        public bool OutBounds(Vector3 position) =>
            _grid.OutBounds(position);
    }
}