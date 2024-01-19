using System.Collections.Generic;
using DefaultNamespace;
using Grid;
using Grid.Elements;
using PathFindingSystem;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Level
{
    public class LevelPN : MonoBehaviour
    {
        [SerializeField] private Vector2Int _sizeGrid;
        [SerializeField] private CameraController _camera;
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileBase _tileBase;
        [SerializeField] private TileBase _tileBase2;


        private Vector3 _startPosition;
        private Vector3 _finishPosition;

        private IGrid _grid;
        private PathFinding _pathFinding;
        private List<CellPFModel> _path;

        private void Awake()
        {
            _grid = new GridPN(_sizeGrid);
            _pathFinding = new PathFinding(_grid);
        }

        private void Start()
        {
            _grid.Create();

            for (int x = 0; x < _grid.Map.GetLength(0); x++)
            {
                for (int y = 0; y < _grid.Map.GetLength(1); y++)
                {
                    var cellPFModel = _grid.Map[x, y];
                    _tilemap.SetTile(cellPFModel.WorldPosition.ToInt(), _tileBase);
                }
            }
        }
        

        private void SetPosition()
        {
            SetStartPosition();

            if (Input.GetMouseButtonUp(0))
            {
                CellPFModel startNode = _grid.FromWorldToCell(_camera.GetWorldPosition());

                if (startNode == null)
                    return;

                _finishPosition = startNode.WorldPosition;
                SetColorElement(startNode);

                FindPath(_startPosition, _finishPosition);
            }
        }

        private void SetStartPosition()
        {
            if (Input.GetMouseButtonDown(0))
            {
                CellPFModel startNode = _grid.FromWorldToCell(_camera.GetWorldPosition());

                if (startNode == null)
                    return;

                _startPosition = startNode.WorldPosition;
                SetColorElement(startNode);
            }
        }


        public List<CellPFModel> FindPath(Vector3 startPosition, Vector3 finishPosition)
        {
            _path = _pathFinding.FindPath(startPosition, finishPosition);
            
            // for (int i = 1; i < _path.Count - 1; i++)
            //     SetColorElement(_path[i]);

            return _path;
        }

        public void SetNotWalkingNode()
        {
            if (Input.GetMouseButtonDown(1))
            {
                CellPFModel cellView = _grid.FromWorldToCell(_camera.GetWorldPosition());
                cellView.SetImpassable();
                SetColorElement(cellView);
            }
        }

        private void ResetElement()
        {
            if (Input.GetMouseButtonDown(2))
            {
                foreach (var cellPFModel in _grid.Map)
                {
                    if (cellPFModel.IsWalkable == false)
                        cellPFModel.SetImpassable(true);

                    SetColorElement(cellPFModel);
                }
            }
        }

        private void SetColorElement(CellPFModel node)
        {
            _tilemap.GetTile(node.GridPosition.ToVector3());
            _tilemap.SetTile(node.GridPosition.ToVector3(), _tileBase2);
        }

        public bool OutBounds(Vector3 position)
        {
            return _grid.OutBounds(position);
        }
    }
}

public class CustomTileBase : TileBase
{
    public Sprite sprite;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = sprite;
    }
}