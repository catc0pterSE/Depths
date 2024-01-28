using System;
using System.Collections.Generic;
using ECS.Scripts.Boot;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace Grid.Elements.Work.Cell
{
    public class CellPFModel
    {
        private List<ProtoPackedEntityWithWorld> _entities = new List<ProtoPackedEntityWithWorld>(10);
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
        public void SetEntities(List<ProtoPackedEntityWithWorld> entities) => 
            _entities = entities;

        public List<ProtoPackedEntityWithWorld> GetEntities() => _entities; 
        public void AddEntity(ProtoPackedEntityWithWorld entity) => _entities.Add(entity);
        public void RemoveEntity(ProtoPackedEntityWithWorld entity) => _entities.Remove(entity);
        public bool HasEntity(ProtoPackedEntityWithWorld entity) => _entities.Contains(entity);
        public bool HasEntity() => _entities.Count > 0;
        public bool HasEntityWithComponent<T>(ProtoPool<T> pool, ProtoWorld protoWorld) where T : struct
        {
            foreach (var packedEntity in _entities)
            {
                packedEntity.Unpack(out protoWorld, out var protoEntity);
 
                if (pool.Has(protoEntity))
                {
                    return true;
                }
            }

            return false;
        }
        
        public bool HasEntityWithComponent(IProtoPool pool, MainAspect aspect, out ProtoEntity entity)
        {
            var protoWorld = aspect.World();
            foreach (var packedEntity in _entities)
            {
                packedEntity.Unpack(out protoWorld, out var protoEntity);
                if (pool.Has(protoEntity))
                {
                    entity = protoEntity;
                    return true;
                }
            }

            entity = default;
            return false;
        }
    }
}