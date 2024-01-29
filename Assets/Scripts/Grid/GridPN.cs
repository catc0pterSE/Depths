using DefaultNamespace;
using Grid.Elements;
using Grid.Elements.Work.Cell;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class GridPN : IGrid
    {
        public CellPFModel[,] Map { get; }

        private Vector2Int Size;
        private readonly Tilemap _tilemapGrid;
        private TileBase _tileBase;

        public GridPN(Vector2Int size, Tilemap tilemapGrid, TileBase tileBase)
        {
            _tilemapGrid = tilemapGrid;
            _tileBase = tileBase;
            Size = size;
            Map = new CellPFModel[size.x, size.y];
        }

        public void Create()
        {
            for (var x = 0; x < Map.GetLength(0); x++)
            {
                for (var y = 0; y < Map.GetLength(1); y++)
                {
                    var cell = new CellPFModel();
                    cell.SetWorldPosition(new Vector3(x, y, 0));
                    cell.SetGritPosition(new Vector2Int(x, y));
                    _tilemapGrid.SetTile(cell.WorldPosition.ToInt(), _tileBase);

                    Map[x, y] = cell;
                }
            }
        }

        public CellPFModel GetCell(Vector2Int position) =>
            Map[position.x, position.y];

        public CellPFModel FromWorldToCell(Vector3 position) => 
            GetCell(position.FloorPositionInt2());

        public bool TryGetCell(Vector3 position, out CellPFModel cell)
        {
            if (TryGetCell(position.FloorPositionInt2(), out cell))
                return true;

            return false;
        }

        public Vector3 ClampPosition(Vector3 position)
        {
            position.x = Mathf.Clamp(position.x, 0, Size.x);
            position.y =  Mathf.Clamp(position.y, 0, Size.y);
            
            return position;   
        }
        public bool OutBounds(Vector3 position)
        {
            var floorPosition = position.FloorPositionInt2();
            
            return floorPosition.x > Size.x ||
                   floorPosition.y > Size.y ||
                   floorPosition.x < 0 ||
                   floorPosition.y < 0;
        }

        public bool OutBounds(Vector2Int gridPosition) => 
            gridPosition.x >= Size.x || gridPosition.y >= Size.y || gridPosition.x < 0 || gridPosition.y < 0;

        public bool TryGetCell(Vector2Int position, out CellPFModel cell)
        {
            cell = null;

            if (position.x >= Size.x || position.y >= Size.y || position.x < 0 || position.y < 0)
                return false;

            cell = GetCell(position);
            return true;
        }
    }
}