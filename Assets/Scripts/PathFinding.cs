using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinding
{
    private const int MoveStraightCost = 10;
    private const int MoveDiagonalCost = 14;

    private readonly IGrid _gridPN;

    private List<CellData> _openList;
    private List<CellData> _closedList;

    public PathFinding(IGrid gridPn) =>
        _gridPN = gridPn;

    public List<Cell> FindPath(Vector3 startPosition, Vector3 finishPosition)
    {
        CellData startCell = _gridPN.Cells[_gridPN.GetElement(startPosition)];
        CellData endCell = _gridPN.Cells[_gridPN.GetElement(finishPosition)];

        _openList = new List<CellData> { startCell };
        _closedList = new List<CellData>();

        SetStatsCell(startCell, 0, endCell);

        while (_openList.Count > 0)
        {
            CellData currentCell = GetLowestTotalCostNode(_openList);

            if (currentCell == endCell)
                return CalculatePath(endCell);

            _openList.Remove(currentCell);
            _closedList.Add(currentCell);

            foreach (var neighbourCell in currentCell.Neighbours)
            {
                if (_closedList.Contains(neighbourCell))
                    continue;

                if (neighbourCell.isWalkable == false)
                {
                    _closedList.Add(neighbourCell);
                    continue;
                }

                int tentativeTransitionCost =
                    currentCell.TransitionCost + CalculateDistanceCost(currentCell, neighbourCell);

                if (tentativeTransitionCost < neighbourCell.TransitionCost)
                {
                    neighbourCell.ComeFromCell = currentCell;
                    SetStatsCell(neighbourCell, tentativeTransitionCost, endCell);

                    if (_openList.Contains(neighbourCell) == false)
                        _openList.Add(neighbourCell);
                }
            }
        }

        return null;
    }

    private List<Cell> CalculatePath(CellData node)
    {
        var path = new List<CellData> { node };

        CellData currentNode = node;
        while (currentNode.ComeFromCell != null)
        {
            path.Add(currentNode.ComeFromCell);
            currentNode = currentNode.ComeFromCell;
        }

        path.Reverse();
        return ConvertColl(path);
    }

    private CellData GetLowestTotalCostNode(IReadOnlyCollection<CellData> openList) => 
        openList.FirstOrDefault(element => element.TotalCost == openList.Min(element => element.TotalCost));

    private int CalculateDistanceCost(CellData firstCell, CellData secondCell)
    {
        var first = GetCell(firstCell);
        var second = GetCell(secondCell);

        int xDistance = Mathf.Abs(first.MapPosition.x - second.MapPosition.x);
        int yDistance = Mathf.Abs(first.MapPosition.y - second.MapPosition.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MoveDiagonalCost * Mathf.Min(xDistance, yDistance) + MoveStraightCost * remaining;
    }

    private void SetStatsCell(CellData nodeStats, int TransitionCost, CellData targetNode)
    {
        nodeStats.SetTransitionCost(TransitionCost);
        nodeStats.SetWayToFinalCell(CalculateDistanceCost(nodeStats, targetNode));
        nodeStats.CalculateTotalCost();
    }

    private Cell GetCell(CellData cellData) =>
        _gridPN.Cells.Where(element => element.Value == cellData).Select(element => _gridPN.GetElement(element.Key.MapPosition))
            .FirstOrDefault();

    private List<Cell> ConvertColl(IEnumerable<CellData> path) =>
        path.Select(GetCell).ToList();
}