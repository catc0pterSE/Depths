using System;
using UnityEngine;

namespace Grid.Elements
{
    public class CellPFModel
    {
        private int _wayToFinalCell;

        public Vector3 WorldPosition { get; private set; }
        public Vector2Int GridPosition { get; private set; }
        public bool IsWalkable { get; private set; } = true;
        public CellPFModel ComeFromCellPf { get; private set; }
        public int TransitionCost { get; private set; } = int.MaxValue;
        public int TotalCost { get; private set; }

        public void SetComeFromCell(CellPFModel comeFromCellPf) => 
            ComeFromCellPf = comeFromCellPf;

        public void SetGritPosition(Vector2Int position) => 
            GridPosition = position;

        public void SetImpassable(bool isWalkable = false) =>
            IsWalkable = isWalkable;

        public void SetWorldPosition(Vector3 worldPosition) => 
            WorldPosition = worldPosition;

        public void SetTransitionCost(int cost) =>
            TransitionCost = cost;

        public void SetWayToFinalCell(int countCells) =>
            _wayToFinalCell = countCells;

        public void CalculateTotalCost() =>
            TotalCost = TransitionCost + _wayToFinalCell;

        public void ResettingIndicators()
        {
            TransitionCost = Int32.MaxValue;
            ComeFromCellPf = null;
        }
    }
}