using System.Collections.Generic;
using System.Linq;
using Grid;
using Grid.Elements;
using UnityEngine;

namespace PathFindingSystem
{
    public class PathFinding
    {
        private const int MoveStraightCost = 10;
        private const int MoveDiagonalCost = 14;

        private readonly IGrid _gridPN;

        private List<CellPFModel> _openList;
        private List<CellPFModel> _closedList;

        public PathFinding(IGrid gridPn) =>
            _gridPN = gridPn;

        public List<CellPFModel> FindPath(Vector3 startPosition, Vector3 finishPosition)
        {
            CellPFModel startCellPf = _gridPN.FromWorldToCell(startPosition);
            CellPFModel endCellPf = _gridPN.FromWorldToCell(finishPosition);

            _openList = new List<CellPFModel> { startCellPf };
            _closedList = new List<CellPFModel>();    

            SetStatsCell(startCellPf, 0, endCellPf);

            while (_openList.Count > 0)
            {
                CellPFModel currentCellView = GetLowestTotalCostNode(_openList);

                if (currentCellView == endCellPf)
                {
                    var cellPath = CalculatePath(endCellPf);
                
                    ResettingIndicators(_openList);
                    ResettingIndicators(_closedList);

                    return cellPath;
                }

                _openList.Remove(currentCellView);
                _closedList.Add(currentCellView);

                foreach (var neighbourCell in Neighbours(currentCellView))
                {
                    if (_closedList.Contains(neighbourCell))
                        continue;

                    if (neighbourCell.IsWalkable == false)
                    {
                        _closedList.Add(neighbourCell);
                        continue;
                    }

                    int tentativeTransitionCost =
                        currentCellView.TransitionCost + CalculateDistanceCost(currentCellView, neighbourCell);

                    if (tentativeTransitionCost < neighbourCell.TransitionCost)
                    {
                        neighbourCell.SetComeFromCell(currentCellView);
                        SetStatsCell(neighbourCell, tentativeTransitionCost, endCellPf);

                        if (_openList.Contains(neighbourCell) == false)
                            _openList.Add(neighbourCell);
                    }
                }
            }

            return null;
        }

        private void ResettingIndicators(List<CellPFModel> list)
        {
            foreach (var cellView in list) 
                cellView.ResettingIndicators();
        }

        private List<CellPFModel> Neighbours(CellPFModel target)
        {
            List<CellPFModel> neighboursCells = new List<CellPFModel>();

            for (int x = target.GridPosition.x - 1; x <= target.GridPosition.x + 1; x++)
            {
                for (int y = target.GridPosition.y - 1; y <= target.GridPosition.y + 1; y++)
                {
                    CellPFModel currentCell = _gridPN.FromWorldToCell(new Vector2Int(x, y));
                
                    if(currentCell == null)
                        continue;
                
                    neighboursCells.Add(currentCell);
                }
            }

            return neighboursCells;
        }
    
        private List<CellPFModel> CalculatePath(CellPFModel cell)
        {
            var path = new List<CellPFModel> { cell };

            CellPFModel currentNode = cell;
            while (currentNode.ComeFromCellPf != null)
            {
                path.Add(currentNode.ComeFromCellPf);
                currentNode = currentNode.ComeFromCellPf;
            }

            path.Reverse();
            return path;
        }

        private CellPFModel GetLowestTotalCostNode(IReadOnlyCollection<CellPFModel> openList) => 
            openList.FirstOrDefault(element => element.TotalCost == openList.Min(element => element.TotalCost));

        private int CalculateDistanceCost(CellPFModel first, CellPFModel second)
        {
            int xDistance = Mathf.Abs(first.GridPosition.x - second.GridPosition.x);
            int yDistance = Mathf.Abs(first.GridPosition.y - second.GridPosition.y);
            int remaining = Mathf.Abs(xDistance - yDistance);

            return MoveDiagonalCost * Mathf.Min(xDistance, yDistance) + MoveStraightCost * remaining;
        }

        private void SetStatsCell(CellPFModel nodeStats, int TransitionCost, CellPFModel targetNode)
        {
            nodeStats.SetTransitionCost(TransitionCost);
            nodeStats.SetWayToFinalCell(CalculateDistanceCost(nodeStats, targetNode));
            nodeStats.CalculateTotalCost();
        }
    
    }
}