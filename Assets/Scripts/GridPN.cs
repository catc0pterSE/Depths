using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridPN : IGrid
{
    private readonly PoolPathCell _poolPathCell;
    private readonly Cell[,] _map;

    public Dictionary<Cell, CellData> Cells { get; }

    public GridPN(Vector2Int size, PoolPathCell poolPathCell)
    {
        _map = new Cell[size.x, size.y];
        _poolPathCell = poolPathCell;
        Cells = new Dictionary<Cell, CellData>();
    }

    public void Create()
    {
        for (var x = 0; x < _map.GetLength(0); x++)
        {
            for (var y = 0; y < _map.GetLength(0); y++)
            {
                var cell = _poolPathCell.GetFreeCell();
                cell.transform.position = new Vector3(x, 0, y);

                _map[x, y] = cell;
                cell.Initialize(x, y);
                
                Cells.Add(cell, new CellData());
                
                SetColorMap(cell, Color.grey);
            }
        }

        SetAllNeighbours();
    }

    private void SetAllNeighbours()
    {
        foreach (var cell in Cells) 
            cell.Value.SetNeighbours(GerNeighbourPosition(cell.Key));
    }

    private List<CellData> GerNeighbourPosition(Cell cell) => 
        GetNeighbourList(cell).Select(currentCell => Cells[currentCell]).ToList();

    private List<Cell> GetNeighbourList(Cell currentNode)
    {
        List<Cell> neighbourList = new List<Cell>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Cell cell =
                    GetElement(new Vector2Int(currentNode.MapPosition.x + x, currentNode.MapPosition.y + y));

                if (cell == null || currentNode == cell)
                    continue;

                neighbourList.Add(cell);
            }
        }

        return neighbourList;
    }

    public Cell GetElement(Vector2Int position)
    {
        if (position.x < 0 || position.y < 0)
            return null;

        if (position.x > _map.GetLength(0) - 1 ||  position.y > _map.GetLength(1) - 1)
            return null;

        return _map[position.x,  position.y];
    }

    public Cell GetElement(Vector3 position)
    {
        Cell cell = null;

        foreach (var element in _map)
            if (element.Position == position)
                cell = element;

        return cell;
    }

    private void SetColorMap(Cell cell, Color color)
    {
        int X = cell.MapPosition.x;
        int Y = cell.MapPosition.y;

        if ((X % 2 != 0 && Y % 2 == 0) || (X % 2 == 0 && Y % 2 != 0))
            cell.SetDefaultColor(color);
    }
}