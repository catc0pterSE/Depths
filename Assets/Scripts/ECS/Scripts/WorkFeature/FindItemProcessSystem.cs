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
    public sealed class DropItem : IProtoRunSystem
    {
        [DI] private readonly MainAspect _aspect;
        [DI] private readonly PathFindingService _path;
        public void Run()
        {
            foreach (var entity in _aspect.ItemsInHandIt)
            {
                ref var item = ref _aspect.ItemsInHand.Get(entity);;
                
                if(item.packedEntity.Unpack(out var world, out var itemEntity))
                {
                    Debug.Log("успешный успех");
                    
                    _aspect.Transforms.Get(itemEntity).value.SetParent(null);
                    
                    _aspect.AddCell.Add(itemEntity);
                
                    _aspect.Sync.Del(itemEntity);
                    
                    _aspect.ItemsInHand.Del(entity);
                    _aspect.Drop.Del(entity);
                }
            }
        }
    }
    public sealed class AddCellSystem : IProtoRunSystem
    {
        [DI] private readonly MainAspect _aspect;
        [DI] private readonly PathFindingService _path;
        public void Run()
        {
            foreach (var protoEntity in _aspect.AddCellIt)
            {
                var grid = _path.Grid;

                ref readonly var position = ref _aspect.Position.Get(protoEntity).value;
                
                var cell = grid.GetCell(position.FloorPosition());

                var pack = _aspect.World().PackEntityWithWorld(protoEntity);
                
                cell.AddEntity(pack);
                
                Debug.Log($"Add Cell {pack.GetHashCode()}");
                Debug.Log($"Cell Info {cell.GetHashCode()}");
                Debug.Log($"Cell G_Pos {cell.GridPosition}");
                Debug.Log($"Cell W_Pos {cell.WorldPosition}");
                
                foreach (var protoPackedEntity in cell.GetEntities())
                {
                    Debug.Log($"unpack:{protoPackedEntity.Unpack(out var world, out var cellEntity)}");
                    Debug.Log($"hashCode:{protoPackedEntity.GetHashCode()}");
                    Debug.Log($"has item {_aspect.Items.Has(cellEntity)}");
                }
                
                
                _aspect.AddCell.Del(protoEntity);

            }
        }
    }
    public sealed class FindItemProcessSystem : IProtoRunSystem
    {
        [DI] private readonly SceneData _sceneData;
        [DI] private readonly RuntimeData _runtimeData;
        [DI] private readonly StaticData _staticData;
        
        
        [DI] private readonly MainAspect _aspect;
        [DI] private PathAspect _pathAspect;
        [DI] private PathFindingService _pathService;
        [DI] private SpatialHash _spatialHash;
        public void Run()
        {
            foreach (var entityCharacter in _aspect.FindItemProcessGet)
            {
                ref readonly var component = ref _aspect.FindItemProcess.Get(entityCharacter);
                ref readonly var position = ref _aspect.Position.Get(entityCharacter).value;

                component.ItemEntity.Unpack(out var world, out var itemEntity);
                
                ref var positionItem = ref _aspect.Position.Get(itemEntity).value;

                var dist = (position - positionItem).sqrMagnitude;

                if (dist < 0.1f)
                {
                    _aspect.ItemsInHand.Add(entityCharacter).packedEntity = component.ItemEntity;   
                    
                    _aspect.Sync.Add(itemEntity);

                    var trItem = _aspect.Transforms.Get(itemEntity).value;
                    
                    var trUnit = _aspect.Transforms.Get(entityCharacter).value;;

                    trItem.SetParent(trUnit);
                          
                    ref var createPath = ref _aspect.PathAspect.CreatePath.GetOrAdd(entityCharacter, out _);
                    createPath.start = position;

                    foreach (var protoEntity in _aspect.ZoneAspect.ZoneIt)
                    {
                        var zone = _aspect.ZoneAspect.Zone.Get(protoEntity);

                        foreach (var cellsValue in zone.Cells.Keys)
                        {
                            var cellInZone = _pathService.Grid.GetCell(cellsValue);
                            
                            if (cellInZone.HasEntityWithComponent(_aspect.Items, _aspect.World()))
                            {
                                continue;
                            }
                            Debug.Log($"o Cell Info {cellInZone.GetHashCode()}");
                            Debug.Log($"o Cell G_Pos {cellInZone.GridPosition}");
                            Debug.Log($"o Cell W_Pos {cellInZone.WorldPosition}");
                            
                            
                            var pathEnd = new Vector3(cellInZone.GridPosition.x, cellInZone.GridPosition.y);
                            
                            createPath.end = pathEnd;

                            positionItem = cellInZone.WorldPosition;
                            
                            _aspect.TargetDrop.GetOrAdd(entityCharacter, out _).value = pathEnd;
                            
                            break;
                        }
                    }

                }
            }

            foreach (var index in _aspect.FindItemProcessDrop)
            {

                ref var position = ref _aspect.Position.Get(index).value;;
                ref var targetDrop = ref _aspect.TargetDrop.Get(index).value;;
                
                var cellPosition = targetDrop.FloorPosition();
                
                foreach (var protoEntity in _aspect.ZoneAspect.ZoneIt)
                {
                    var zone = _aspect.ZoneAspect.Zone.Get(protoEntity);
                    if(!zone.Cells.ContainsKey(cellPosition))
                        continue;

                    var cell = _pathService.Grid.GetCell(cellPosition);
                    
                    if (cell.HasEntityWithComponent(_aspect.Items, _aspect.World()))
                    {
                        bool find = false;
                        foreach (var cellsValue in zone.Cells.Keys)
                        {
                            var cellInZone = _pathService.Grid.GetCell(cellsValue);
                            
                            if (cellInZone.HasEntityWithComponent(_aspect.Items, _aspect.World()))
                            {
                                continue;
                            }

                            find = true;
                            
                            ref var createPath = ref _aspect.PathAspect.CreatePath.GetOrAdd(index, out _);
                            createPath.start = position;
                            
                            var pathEnd = cellInZone.WorldPosition;


                            ref var item = ref _aspect.ItemsInHand.Get(index).packedEntity;

                            item.Unpack(out var world, out var entity);
                            
                            _aspect.Position.Get(entity).value = pathEnd;
                            
                            createPath.end = pathEnd;
                            targetDrop = pathEnd;
                            
                            break;
                        }

                        if (!find)
                        {
                            targetDrop = position;
                            
                            ref var item = ref _aspect.ItemsInHand.Get(index).packedEntity;

                            item.Unpack(out var world, out var entity);
                            
                            _aspect.Position.Get(entity).value = position;
                        }
                    }
                }
                

                var dist = position.FastDistance(targetDrop);

                if (dist < 0.01f)
                {
                    _aspect.CancelWork.Add(index);
                }

            }

            foreach (var index in _aspect.FindItemProcessCancel)
            {
                _aspect.Drop.Add(index);
                _aspect.TargetDrop.Del(index);
                _aspect.FindItemProcess.Del(index);
            }
        }
        
    }
}