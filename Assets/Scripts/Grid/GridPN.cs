using Grid.Elements;
using Pool;
using UnityEngine;

namespace Grid
{
    public class GridPN : IGrid
    {
        public CellPFModel[,] Map { get; }

        public Vector2Int Size;

        public GridPN(Vector2Int size)
        {
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

                    Map[x, y] = cell;
                }
            }
        }

        public CellPFModel GetCell(Vector2Int position) =>
            Map[position.x, position.y];

        public CellPFModel FromWorldToCell(Vector3 position)
        {
            var positionInt = WorldPositionToCellPosition(position);
            
            return GetCell(positionInt);
        }


        public Vector2Int WorldPositionToCellPosition(Vector3 position)
        {
            return new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
        }
        
        public bool TryGetCell(Vector3 position, out CellPFModel cell)
        {
            var positionInt = WorldPositionToCellPosition(position);

            if (TryGetCell(positionInt, out cell))
            {
                return true;
            }
            
            return false;
        }

        public bool OutBounds(Vector3 position)
        {
            var gridPosition = WorldPositionToCellPosition(position);
            
            return gridPosition.x >= Size.x || gridPosition.y >= Size.y || gridPosition.x < 0 || gridPosition.y < 0;
        }
        
        public bool OutBounds(Vector2Int gridPosition)
        {
            return gridPosition.x >= Size.x || gridPosition.y >= Size.y || gridPosition.x < 0 || gridPosition.y < 0;
        }
        
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