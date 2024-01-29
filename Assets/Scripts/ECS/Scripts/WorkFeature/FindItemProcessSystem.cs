using DefaultNamespace;
using ECS.Scripts.Boot;
using ECS.Scripts.Data;
using ECS.Scripts.GeneralComponents;
using ECS.Scripts.Path.Component;
using ECS.Scripts.PathFeature.Systems;
using ECS.Scripts.ProviderComponents;
using ECS.Scripts.TestSystem;
using Grid.Elements.Work.Cell;
using Leopotam.Ecs;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Level;
using UnityEngine;

namespace ECS.Scripts.WorkFeature
{
    public sealed class FindItemProcessSystem : IProtoRunSystem
    {
        [DI] private readonly SceneData _sceneData;
        [DI] private readonly RuntimeData _runtimeData;
        [DI] private readonly StaticData _staticData;
        
        
        [DI] private readonly MainAspect _mainAspect;
        [DI] private PathAspect _pathAspect;
        [DI] private PathFindingService _pathService;
        [DI] private SpatialHash _spatialHash;
        public void Run()
        {
            CheckDistanceGetItem();
            
            FindZoneForDrop();
            
            CheckDistanceDrop();

            CancelWork();
        }

        private void CheckDistanceGetItem()
        {
            foreach (var entityProcess in _mainAspect.FindItemProcessGet)
            {
                ref var findProcess = ref _mainAspect.FindItemProcess.Get(entityProcess);
                
                if(findProcess.ItemInHand) continue;
                
                ref readonly var owner = ref _mainAspect.Owners.Get(entityProcess).value;
                
                owner.Unpack(_mainAspect.World(), out var ownerEntity);

                if (_mainAspect.ItemsInHand.Has(ownerEntity))
                {
                    findProcess.ItemInHand = true;
                    continue;
                }
                
                ref readonly var position = ref _mainAspect.Position.Get(ownerEntity).value;
                
                ref readonly var packedEntity = ref _mainAspect.TargetWork.Get(entityProcess).PackedEntity;
                
                packedEntity.Unpack(_mainAspect.World(), out var itemEntity);
                
                ref readonly var positionItem = ref _mainAspect.Position.Get(itemEntity).value;
                
                var dist = position.FastDistance(positionItem);

                if (dist < 0.1f)
                {
                    var trItem = _mainAspect.Transforms.Get(itemEntity).value;
                    
                    var trUnit = _mainAspect.Transforms.Get(ownerEntity).value;;

                    trItem.SetParent(trUnit);
                    
                    _mainAspect.Sync.Add(itemEntity);
                    _mainAspect.ItemsInHand.Add(ownerEntity).packedEntity = packedEntity;

                    findProcess.ItemInHand = true;
                }
            }
        }

        private void FindZoneForDrop()
        {
            foreach (var entityProcess in _mainAspect.FindZoneForItemIt)
            {
                ref var findProcess = ref _mainAspect.FindItemProcess.Get(entityProcess);
                
                ref readonly var owner = ref _mainAspect.Owners.Get(entityProcess).value;
                
                owner.Unpack(_mainAspect.World(), out var ownerEntity);
                
                if(!findProcess.ItemInHand) continue;
                
                ref var position = ref _mainAspect.Position.Get(ownerEntity).value;;
                
                ref var targetDrop = ref _mainAspect.TargetDrop.GetOrAdd(entityProcess, out bool added).value;
                
                var cellPosition = targetDrop.FloorPositionInt2();
                
                if (added)
                {
                    foreach (var protoEntity in _mainAspect.ZoneAspect.ZoneIt)
                    {
                        var zone = _mainAspect.ZoneAspect.Zone.Get(protoEntity);

                        foreach (var gridPosition in zone.Cells.Keys)
                        {
                            var cellInZone = _pathService.Grid.GetCell(gridPosition);
                            if (cellInZone.HasEntityWithComponent(_mainAspect.Items, _mainAspect.World()))
                            {
                                continue;
                            }
                            
                            ref var createPath = ref _mainAspect.PathAspect.CreatePath.GetOrAdd(ownerEntity, out _);
                            
                            createPath.start = position;
                            
                            var pathEnd = cellInZone.WorldPosition;
                            
                            createPath.end = pathEnd;
                            
                            targetDrop = pathEnd;
                        }
    
                    }
                }
                
                foreach (var protoEntity in _mainAspect.ZoneAspect.ZoneIt)
                {
                    var zone = _mainAspect.ZoneAspect.Zone.Get(protoEntity);
                    if(!zone.Cells.ContainsKey(cellPosition))
                        continue;

                    var cell = _pathService.Grid.GetCell(cellPosition);
                    
                    if (cell.HasEntityWithComponent(_mainAspect.Items, _mainAspect.World()))
                    {
                        bool find = false;
                        foreach (var cellsValue in zone.Cells.Keys)
                        {
                            var cellInZone = _pathService.Grid.GetCell(cellsValue);
                            
                            if (cellInZone.HasEntityWithComponent(_mainAspect.Items, _mainAspect.World()))
                            {
                                continue;
                            }

                            find = true;
                            
                            ref var createPath = ref _mainAspect.PathAspect.CreatePath.GetOrAdd(ownerEntity, out _);
                            
                            createPath.start = position;
                            
                            var pathEnd = cellInZone.WorldPosition;
                            
                            createPath.end = pathEnd;
                            
                            targetDrop = pathEnd;
                            
                            break;
                        }

                        if (!find)
                        {
                            targetDrop = position;
                            
                            _mainAspect.Drop.Add(ownerEntity);
                            
                        }
                    }
                }
            }
        }

        private void CheckDistanceDrop()
        {
            foreach (var entityProcess in _mainAspect.FindItemProcessDrop)
            {
                ref readonly var owner = ref _mainAspect.Owners.Get(entityProcess).value;
                owner.Unpack(_mainAspect.World(), out var ownerEntity);
                
                ref var position = ref _mainAspect.Position.Get(ownerEntity).value;;
                
                ref var targetDrop = ref _mainAspect.TargetDrop.Get(entityProcess).value;;

                var dist = position.FastDistance(targetDrop);

                if (dist < 0.01f)
                {
                    _mainAspect.CancelWork.Add(entityProcess);
                }
            }
        }

        private void CancelWork()
        {
            foreach (var entityProcess in _mainAspect.FindItemProcessCancel)
            {
                ref readonly var owner = ref _mainAspect.Owners.Get(entityProcess).value;
                owner.Unpack(_mainAspect.World(), out var ownerEntity);
                
                _mainAspect.Drop.Add(ownerEntity);
                
                _mainAspect.CurrentWork.Del(ownerEntity);
            }
        }
    }
}