using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grid.Elements.Work.Cell
{
    public class CellPFModel
    {
        private List<Entity> _entities;
        private int _wayToFinalCell;
        public Vector3 WorldPosition { get; private set; }
        public Vector2Int GridPosition { get; private set; }
        public bool Obstacle { get; private set; } = true;
        public CellPFModel ComeFromCellPf { get; private set; }
        public int TransitionCost { get; private set; } = int.MaxValue;
        public int TotalCost { get; private set; }
        public void SetComeFromCell(CellPFModel comeFromCellPf) => 
            ComeFromCellPf = comeFromCellPf;

        public void SetGritPosition(Vector2Int position) => 
            GridPosition = position;

        public void SetImpassable(bool isWalkable = false) =>
            Obstacle = isWalkable;

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

        public void SetEntities(List<Entity> entities) => 
            _entities = entities;

        public List<Entity> GetEntities() => 
            new(_entities);
    }
}