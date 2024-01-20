using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Jobs;

public class NewPathfinding : MonoBehaviour
{
    private const int MoveStraightCost = 10;
    private const int MoveDiagonalCost = 14;

    [BurstCompile]
    private struct FindPathJob : IJob
    {
        public int2 startPosition;
        public int2 finishPosition;

        public void Execute()
        {
            int2 gridSize = new int2(4, 4);

            NativeArray<Cell> pathCell = new NativeArray<Cell>(gridSize.x * gridSize.y, Allocator.Temp);

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    Cell cell = new Cell();
                    cell.x = x;
                    cell.y = y;
                    cell.index = CalculateIndex(x, y, gridSize.x);

                    cell.gCost = int.MaxValue;
                    cell.hCost = CalculateDistanceCost(new int2(x, y), finishPosition);
                    cell.CalculateFCost();

                    cell.isWalkable = true;
                    cell.cameFromCellIndex = -1;

                    pathCell[cell.index] = cell;
                }
            }

            NativeArray<int2> neighbourOffsetArray = new NativeArray<int2>(8, Allocator.Temp);
            neighbourOffsetArray[0] = new int2(-1, 0);
            neighbourOffsetArray[1] = new int2(+1, 0);
            neighbourOffsetArray[2] = new int2(0, +1);
            neighbourOffsetArray[3] = new int2(0, -1);
            neighbourOffsetArray[4] = new int2(-1, -1);
            neighbourOffsetArray[5] = new int2(-1, +1);
            neighbourOffsetArray[6] = new int2(+1, -1);
            neighbourOffsetArray[7] = new int2(+1, +1);

            int endCellIndex = CalculateIndex(finishPosition.x, finishPosition.y, gridSize.x);

            Cell startCell = pathCell[CalculateIndex(startPosition.x, startPosition.y, gridSize.x)];
            startCell.gCost = 0;
            startCell.CalculateFCost();
            pathCell[startCell.index] = startCell;

            NativeList<int> openList = new NativeList<int>(Allocator.Temp);
            NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

            openList.Add(startCell.index);

            while (openList.Length > 0)
            {
                int currentCellIndex = GetLowestCostFCellIndex(openList, pathCell);
                Cell currentCell = pathCell[currentCellIndex];

                if (currentCellIndex == endCellIndex)
                {
                    break;
                }

                for (int i = 0; i < openList.Length; i++)
                {
                    if (openList[i] == currentCellIndex)
                    {
                        openList.RemoveAtSwapBack(i);
                        break;
                    }
                }

                closedList.Add(currentCellIndex);

                for (int i = 0; i < neighbourOffsetArray.Length; i++)
                {
                    int2 neighbourOffset = neighbourOffsetArray[i];
                    int2 neighbourPosition =
                        new int2(currentCell.x + neighbourOffset.x, currentCell.y + neighbourOffset.y);

                    if (!IsPositionInsideGrid(neighbourPosition, gridSize))
                        continue;

                    int neighbourCellIndex = CalculateIndex(neighbourPosition.x, neighbourPosition.y, gridSize.x);

                    if (closedList.Contains(neighbourCellIndex))
                        continue;

                    Cell neighbourCell = pathCell[neighbourCellIndex];

                    if (!neighbourCell.isWalkable)
                        continue;

                    int2 currentCellPosition = new int2(currentCell.x, currentCell.y);

                    int tentativeGCost =
                        currentCell.gCost + CalculateDistanceCost(currentCellPosition, neighbourPosition);

                    if (tentativeGCost < neighbourCell.gCost)
                    {
                        neighbourCell.cameFromCellIndex = currentCellIndex;
                        neighbourCell.gCost = tentativeGCost;
                        neighbourCell.CalculateFCost();
                        pathCell[neighbourCellIndex] = neighbourCell;

                        if (!openList.Contains(neighbourCell.index))
                            openList.Add(neighbourCell.index);
                    }
                }
            }

            Cell endCell = pathCell[endCellIndex]; // ????????

            if (endCell.cameFromCellIndex == -1)
            {
                Debug.Log("I don`t have path!");
            }
            else
            {
                NativeList<int2> path = CalculatePath(pathCell, endCell);

                foreach (var pathPosition in path)
                    Debug.Log(pathPosition);

                path.Dispose();
            }

            neighbourOffsetArray.Dispose();
            pathCell.Dispose();
            openList.Dispose();
            closedList.Dispose();
        }

        private int CalculateIndex(int x, int y, int gridWidth) =>
            x + y * gridWidth;

        private int CalculateDistanceCost(int2 aPosition, int2 bPosition)
        {
            int xDistance = math.abs(aPosition.x - bPosition.x);
            int yDistance = math.abs(aPosition.y - bPosition.y);
            int remaining = math.abs(xDistance - yDistance);
            return MoveDiagonalCost * math.min(xDistance, yDistance) + MoveStraightCost * remaining;
        }

        private int GetLowestCostFCellIndex(NativeList<int> openList, NativeArray<Cell> pathCellArray)
        {
            Cell lowestCostPathCell = pathCellArray[openList[0]];
            for (int i = 1; i < openList.Length; i++)
            {
                Cell testCell = pathCellArray[openList[i]];
                if (testCell.fCost < lowestCostPathCell.fCost) lowestCostPathCell = testCell;
            }

            return lowestCostPathCell.index;
        }

        private NativeList<int2> CalculatePath(NativeArray<Cell> pathCellArray, Cell endCell)
        {
            if (endCell.cameFromCellIndex == -1)
                return new NativeList<int2>();

            NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
            path.Add(new int2(endCell.x, endCell.y));

            Cell currentCell = endCell;
            while (currentCell.cameFromCellIndex != -1)
            {
                Cell cameFromCell = pathCellArray[currentCell.cameFromCellIndex];
                path.Add(new int2(cameFromCell.x, cameFromCell.y));
                currentCell = cameFromCell;
            }

            return path;
        }

        private bool IsPositionInsideGrid(int2 gridPosition, int2 gridSize)
        {
            return
                gridPosition.x >= 0 &&
                gridPosition.y >= 0 &&
                gridPosition.x < gridSize.x &&
                gridPosition.y < gridSize.y;
        }

        private struct Cell
        {
            public int x;
            public int y;

            public int index;

            public int gCost;
            public int hCost;
            public int fCost;

            public bool isWalkable;

            public int cameFromCellIndex;

            public void CalculateFCost() =>
                fCost = gCost + hCost;
        }
    }
}