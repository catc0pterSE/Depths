using System.Collections.Generic;
using UnityEngine;

public interface IGrid
{
    public Dictionary<Cell, CellData> Cells { get; }
    public void Create();
    public Cell GetElement(Vector2Int position);
    public Cell GetElement(Vector3 position);
}