using Grid.Elements;
using Pool;
using UnityEngine;

namespace Grid
{
    public class GridPN : IGrid
    {
        public CellPFModel[,] Map { get; }

        public GridPN(Vector2Int size)
        {
            Map = new CellPFModel[size.x, size.y];
        }

        public void Create()
        {
            for (var x = 0; x < Map.GetLength(0); x++)
            {
                for (var y = 0; y < Map.GetLength(0); y++)
                {
                    var cell = new CellPFModel();
                    cell.SetWorldPosition(new Vector3(x, y, 0));
                    cell.SetGritPosition(new Vector2Int(x, y));

                    Map[x, y] = cell;
                }
            }
        }

        public CellPFModel FromWorldToCell(Vector2Int position) =>
            Map[position.x, position.y];

        public CellPFModel FromWorldToCell(Vector3 position)
        {
            Vector2Int worldPosition = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
            return FromWorldToCell(worldPosition);
        }

        public bool TryGetCell(Vector2Int position, out CellPFModel cell)
        {
            cell = null;

            if (position.x < 0 || position.y < 0)
                return false;

            if (position.x > Map.GetLength(0) - 1 || position.y > Map.GetLength(1) - 1)
                return false;

            cell = FromWorldToCell(position);
            return true;
        }
    }
}