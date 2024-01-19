using Grid.Elements;
using UnityEngine;

namespace Grid
{
    public interface IGrid
    {
        public CellPFModel[,] Map { get; }
        public void Create();
        public CellPFModel GetCell(Vector2Int position);
        public CellPFModel FromWorldToCell(Vector3 position);
        public bool OutBounds(Vector2Int gridPosition);
        public bool OutBounds(Vector3 gridPosition);
        
        public bool TryGetCell(Vector2Int position, out CellPFModel cell);
        public bool TryGetCell(Vector3 position, out CellPFModel cell);
    }
}