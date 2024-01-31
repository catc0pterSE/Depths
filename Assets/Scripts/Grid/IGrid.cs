using Grid.Elements;
using UnityEngine;

namespace Grid
{
    public interface IGrid
    {
        public CellPFModel[,] Map { get; }
        public void Create();
        public CellPFModel FromWorldToCell(Vector2Int position);
        public CellPFModel FromWorldToCell(Vector3 position);
        public bool TryGetCell(Vector2Int position, out CellPFModel cell);
    }
}