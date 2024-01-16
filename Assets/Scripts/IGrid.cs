using System.Collections.Generic;
using UnityEngine;

public interface IGrid
{
    public Dictionary<CellView, CellData> Cells { get; }
    public void Create();
    public CellView GetElement(Vector2Int position);
    public CellView GetElement(Vector3 position);
}