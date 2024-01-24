using System.Collections.Generic;
using Grid;
using Grid.Elements;
using Grid.Elements.Work.Cell;
using UnityEngine;
using UnityEngine.Pool;

namespace PathFindingSystem
{
    public class PathFinding : IPathFinding
    {
        private const int MoveStraightCost = 10;
        private const int MoveDiagonalCost = 14;

        private readonly IGrid _gridPN;

        private readonly List<CellPFModel> _openList = new List<CellPFModel>(100);
        private readonly List<CellPFModel> _closedList = new List<CellPFModel>(100);
        
        private readonly List<CellPFModel> _search = new List<CellPFModel>(100);

        public PathFinding(IGrid gridPn) =>
            _gridPN = gridPn;
       
        
        public List<Vector3> FindPath(Vector3Int startPosition, Vector3Int finishPosition)
        {
            CellPFModel startCellPf = _gridPN.FromWorldToCell(startPosition);
            CellPFModel endCellPf = _gridPN.FromWorldToCell(finishPosition);

            _openList.Add(startCellPf);

            SetStatsCell(startCellPf, 0, endCellPf);

            while (_openList.Count > 0)
            {
                CellPFModel currentCellView = GetLowestTotalCostNode(_openList);

                if (currentCellView == endCellPf)
                {
                    break;
                }

                _openList.Remove(currentCellView);
                _closedList.Add(currentCellView);

                foreach (var neighbourCell in Neighbours(currentCellView))
                {
                    if (_closedList.Contains(neighbourCell))
                        continue;

                    if (neighbourCell.Obstacle == false)
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
                
                neighboursCells.Clear();
            }
            
            var cellPath = CalculatePath(_search, endCellPf);
                
            ResettingIndicators(_openList);
            ResettingIndicators(_closedList);
            
            _openList.Clear();
            _closedList.Clear();

            return cellPath;
        }

        private void ResettingIndicators(List<CellPFModel> list)
        {
            foreach (var cellView in list) 
                cellView.ResettingIndicators();
        }

        List<CellPFModel> neighboursCells = new List<CellPFModel>(9);
        
        private List<CellPFModel> Neighbours(CellPFModel target)
        {
            for (int x = target.GridPosition.x - 1; x <= target.GridPosition.x + 1; x++)
            {
                for (int y = target.GridPosition.y - 1; y <= target.GridPosition.y + 1; y++)
                {
                    _gridPN.TryGetCell(new Vector2Int(x, y), out var currentCell);
                
                    if(currentCell == null)
                        continue;
                
                    neighboursCells.Add(currentCell);
                }
            }

            return neighboursCells;
        }
    
        private List<Vector3> CalculatePath( List<CellPFModel> search, CellPFModel cell)
        {
            var path = ListPool<Vector3>.Get();;
            path.Add(cell.WorldPosition);
            CellPFModel currentNode = cell;
            while (currentNode.ComeFromCellPf != null)
            {
                path.Add(currentNode.ComeFromCellPf.WorldPosition);
                currentNode = currentNode.ComeFromCellPf;
            }

            return path;
        }

        private CellPFModel GetLowestTotalCostNode(List<CellPFModel> openList)
        {
            var lowestCostPathCell = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                var testCell = openList[i];
                if (testCell.TotalCost < lowestCostPathCell.TotalCost) lowestCostPathCell = testCell;
            }

            return lowestCostPathCell;
        }

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