using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridPN : IGrid
{
    private readonly PoolPathCell _poolPathCell;
    
    private readonly CellView[,] _map;
    public Dictionary<CellView, CellData> Cells { get; }
    public GridPN(Vector2Int size, PoolPathCell poolPathCell)
    {
        _map = new CellView[size.x, size.y];
        _poolPathCell = poolPathCell;
        Cells = new Dictionary<CellView, CellData>();
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

                var cellData = new CellData();
                cell.Data = cellData;
                Cells.Add(cell, cellData);
                
                SetColorMap(cell, Color.grey);
            }
        }

        SetAllNeighbours();
    }

    private void SetAllNeighbours()
    {
        // foreach (var cell in Cells) 
        //     cell.Value.SetNeighbours(GerNeighbourPosition(cell.Key));
    }

    private List<CellData> GerNeighbourPosition(CellView cellView) => 
        GetNeighbourList(cellView).Select(currentCell => Cells[currentCell]).ToList();

    private List<CellView> GetNeighbourList(CellView currentNode)
    {
        List<CellView> neighbourList = new List<CellView>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                CellView cellView =
                    GetElement(new Vector2Int(currentNode.MapPosition.x + x, currentNode.MapPosition.y + y));

                if (cellView == null || currentNode == cellView)
                    continue;

                neighbourList.Add(cellView);
            }
        }

        return neighbourList;
    }

    public CellView GetElement(Vector2Int position)
    {
        if (position.x < 0 || position.y < 0)
            return null;

        if (position.x > _map.GetLength(0) - 1 ||  position.y > _map.GetLength(1) - 1)
            return null;

        return _map[position.x,  position.y];
    }

    public CellView GetElement(Vector3 position)
    {
        CellView cellView = null;

        foreach (var element in _map)
            if (element.Position == position)
                cellView = element;

        return cellView;
    }

    private void SetColorMap(CellView cellView, Color color)
    {
        int X = cellView.MapPosition.x;
        int Y = cellView.MapPosition.y;

        if ((X % 2 != 0 && Y % 2 == 0) || (X % 2 == 0 && Y % 2 != 0))
            cellView.SetDefaultColor(color);
    }
}