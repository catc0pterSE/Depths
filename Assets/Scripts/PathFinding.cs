using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    private const int MoveStraightCost = 10;
    private const int MoveDiagonalCost = 14;

    private readonly IGrid _gridPN;
        
    private List<PathNode> _openList;
    private List<PathNode> _closedList;

    public PathFinding(IGrid gridPn) =>
        _gridPN = gridPn;

    public List<PathNode> FindPath(Vector3 startPosition, Vector3 finishPosition)
    {
        PathNode startNode = _gridPN.GetElement(startPosition);
        PathNode endNode = _gridPN.GetElement(finishPosition);

        _openList = new List<PathNode> { startNode };
        _closedList = new List<PathNode>();

        SetStatsNode(startNode, 0, endNode);

        while (_openList.Count > 0)
        {
            PathNode currentNode = GetLowestTotalCostNode(_openList);

            if (currentNode == endNode)
                return CalculatePath(endNode);

            _openList.Remove(currentNode);
            _closedList.Add(currentNode);

            foreach (var neighbourNode in GetNeighbourList(currentNode))
            {
                if (_closedList.Contains(neighbourNode))
                    continue;

                if (neighbourNode.isWalkable == false)
                {
                    _closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeTransitionCost =
                    currentNode.TransitionCost + CalculateDistanceCost(currentNode, neighbourNode);

                if (tentativeTransitionCost < neighbourNode.TransitionCost)
                {
                    neighbourNode.ComeFromNode = currentNode;
                    SetStatsNode(neighbourNode, tentativeTransitionCost, endNode);

                    if (_openList.Contains(neighbourNode) == false)
                        _openList.Add(neighbourNode);
                }
            }
        }

        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                PathNode pathNode =
                    _gridPN.GetElement(currentNode.ArrayPosition.x + x, currentNode.ArrayPosition.y + y);

                if (pathNode == null || currentNode == pathNode)
                    continue;

                neighbourList.Add(pathNode);
            }
        }

        return neighbourList;
    }

    private List<PathNode> CalculatePath(PathNode node)
    {
        var path = new List<PathNode> { node };

        PathNode currentNode = node;
        while (currentNode.ComeFromNode != null)
        {
            path.Add(currentNode.ComeFromNode);
            currentNode = currentNode.ComeFromNode;
        }

        path.Reverse();
        return path;
    }

    private PathNode GetLowestTotalCostNode(List<PathNode> openList)
    {
        PathNode lowestTotalCostNode = openList[0];

        for (int i = 1; i < openList.Count; i++)
            if (openList[i].TotalCost < lowestTotalCostNode.TotalCost)
                lowestTotalCostNode = openList[i];

        return lowestTotalCostNode;
    }

    private int CalculateDistanceCost(PathNode first, PathNode second)
    {
        int xDistance = Mathf.Abs(first.ArrayPosition.x - second.ArrayPosition.x);
        int yDistance = Mathf.Abs(first.ArrayPosition.y - second.ArrayPosition.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MoveDiagonalCost * Mathf.Min(xDistance, yDistance) + MoveStraightCost * remaining;
    }

    private void SetStatsNode(PathNode nodeStats, int TransitionCost, PathNode targetNode)
    {
        nodeStats.SetTransitionCost(TransitionCost);
        nodeStats.SetWayToFinalCell(CalculateDistanceCost(nodeStats, targetNode));
        nodeStats.CalculateTotalCost();
    }
}