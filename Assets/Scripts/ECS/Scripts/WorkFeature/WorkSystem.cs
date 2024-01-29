
using DefaultNamespace;
using ECS.Scripts.Boot;
using ECS.Scripts.Data;
using ECS.Scripts.TestSystem;
using Leopotam.EcsProto;
using Leopotam.EcsProto.Ai.Utility;
using Leopotam.EcsProto.QoL;
using Level;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ECS.Scripts.WorkFeature
{

    public sealed class MineDiedSystem : IProtoRunSystem
    {
        [DI]  private readonly StaticData _staticData;
        [DI] private readonly MainAspect _aspect;
        [DI] private readonly PathFindingService _path;
        
        public void Run()
        {
            foreach (var entityMine in _aspect.MiningDied)
            {
                ref var health = ref _aspect.Health.Get(entityMine).value;
                
                if(health <= 0)
                {
                    _aspect.MiningTag.Del(entityMine);
                    _aspect.Transforms.Get(entityMine).value.gameObject.SetActive(false);
                    
                    Debug.Log("mining die");
                    
                    ref readonly var position = ref _aspect.Position.Get(entityMine).value;
                    
                    var instanceObject = Object.Instantiate(_staticData.ItemPrefab);
            
                    var entityUnit = _aspect.World().NewEntity();
                    
                    _aspect.SelectionAspect.CanSelect.Add(entityUnit);
                    _aspect.Items.Add(entityUnit);
                    _aspect.Position.Add(entityUnit).value = position;
                    _aspect.Transforms.Add(entityUnit).value = instanceObject.transform;

                    var packedEntity = _aspect.World().PackEntityWithWorld(entityUnit);

                    var floor = position.FloorPositionInt2();
                    
                    _path.Grid.Map[floor.x, floor.y].AddEntity(packedEntity);

                }
            }
        }

        
    }
    
    public sealed class UtilityCreateRequestSystem : IProtoRunSystem {
        [DI] readonly MainAspect _mainAspect = default;
        [DI] readonly AiUtilityModuleAspect _aiUtility = default;

        public void Run () 
        {
            foreach (var entity in _mainAspect.AISolutionIt)
            {
                _aiUtility.Request (entity);
            }
        }
    }

    public sealed class UtilityApplySystem : IProtoRunSystem
    {
        [DI] readonly AiUtilityModuleAspect _aiUtility = default;
        [DI] readonly MainAspect _mainAspect = default;
        public void Run()
        {
            foreach (var entity in _mainAspect.AISolutionResponceIt)
            {
                ref var res = ref _aiUtility.ResponseEvent.Get(entity);
                res.Solver.Apply (entity);
            }
        }
    }
    public sealed class HungrySystem : IProtoRunSystem
    {
        [DI] private readonly MainAspect _mainAspect = default;
        [DI] private readonly RuntimeData _runtimeData = default;
        public void Run()
        {
            foreach (var protoEntity in _mainAspect.StatAspect.StatsIt)
            {
                var stats = _mainAspect.StatAspect.Stats.Get(protoEntity).value;
               
                stats[StatType.Hungry].Unpack(_mainAspect.World() ,out var statEntity);
                
                ref var amountHungry = ref _mainAspect.StatAspect.Stat.Get(statEntity).value;

                amountHungry -= _runtimeData.deltaTime;
            }
        }
    }
    public struct FindFood
    {
    }
    public struct AISolution
    {
        public ProtoPackedEntity packedEntity;
    }

    public sealed class FindNearElementSystem : IProtoRunSystem
    {
        [DI] private readonly MainAspect _aspect;
                      
        [BurstCompile]
        private struct DistanceJob : IJob
        {
            public NativeArray<DistanceData> Distance;
            public NativeArray<DistanceData> DistanceOut;
            
            [ReadOnly]  public float3 Position;
            
            public float localDistance;
            
            [BurstCompile]
            public void Execute()
            {
                for (int i = 0; i < Distance.Length; i++)
                {
                    var element = Distance[i];
                    var distanceTwo =  math.distance(element.PositionItem, Position);
                    
                    
                    if (localDistance > distanceTwo)
                    {
                        localDistance = distanceTwo;
                        
                        var distanceOut = DistanceOut[0];
                        
                        distanceOut.PositionItem = element.PositionItem;
                        distanceOut.IndexEntity = element.IndexEntity;

                        DistanceOut[0] = distanceOut;
                    }
                    
                }
            }
        }
        public struct DistanceData
        {
            public float3 PositionItem;
            public ProtoEntity IndexEntity;
        }
        public void Run()
        {
            foreach (var entity in _aspect.FindNearElementIt)
            {
                ref var find = ref _aspect.FindNearElement.Get(entity);
                
                ref readonly var position = ref _aspect.Position.Get(entity).value;

                var length = find.Iterator.Len();

                if (length == 0)
                {
                  // Cancel work
                  return;
                }
                
                var positionNative = new NativeArray<DistanceData>(length, Allocator.TempJob);
                var positionNativeOut = new NativeArray<DistanceData>(1, Allocator.TempJob);

                var distanceDataLoad = new DistanceData();
                distanceDataLoad.IndexEntity = entity;
                positionNativeOut[0] = distanceDataLoad;

                int index = 0;
                
                for (find.Iterator.Begin (); find.Iterator.Next ();) 
                {
                    var distanceData = new DistanceData();
                    distanceData.PositionItem = _aspect.Position.Get(find.Iterator.Entity()).value;
                    distanceData.IndexEntity = find.Iterator.Entity();
                    positionNative[index] = distanceData;

                    index++;
                }

                DistanceJob moveJob = new DistanceJob { Position = position, DistanceOut = positionNativeOut, Distance = positionNative, localDistance = 100000f};

                var jobHandle = moveJob.Schedule();
            
                jobHandle.Complete();
            
                var findEntity = positionNativeOut[0].IndexEntity;
            
                _aspect.ItemsBusy.Add(findEntity);
                
                _aspect.TargetWork.Add(entity).PackedEntity = _aspect.World().PackEntity(findEntity);
                
                _aspect.Owners.Get(entity).value.Unpack(_aspect.World(), out var enityOwner);
                
                ref var createPath = ref _aspect.PathAspect.CreatePath.GetOrAdd(enityOwner, out _);
                createPath.start = position;
                createPath.end = positionNativeOut[0].PositionItem;

                positionNative.Dispose();
                positionNativeOut.Dispose();
            }
        }
    }
    
    public sealed class SearchFoodSolver : IAiUtilitySolver 
    {
        [DI] private readonly MainAspect _mainAspect = default;
        public float Solve(ProtoEntity entity)
        {
            ref var stats = ref _mainAspect.StatAspect.Stats.Get(entity).value;
            
            stats[StatType.Hungry].Unpack(_mainAspect.World() ,out var statEntity);
            
            var stat = _mainAspect.StatAspect.Stat.Get(statEntity).value;
            
            if (stat < 70f) 
            {
                return 2f;
            }
            
            return 0f;
        }
        public void Apply(ProtoEntity entity)
        {
            _mainAspect.AddSolution(_mainAspect.FindFood, entity, out var aiSolution);
        }
    }
    
    public sealed class RandSolver : IAiUtilitySolver 
    {
        [DI] private readonly MainAspect _mainAspect = default;
        public float Solve(ProtoEntity entity)
        {
            return 0.1f;
        }
        public void Apply(ProtoEntity entity)
        {
            _mainAspect.AddSolution(_mainAspect.RandMove, entity, out _);
        }
    }
    
    public sealed class BeginCompleteWorkSolver : IAiUtilitySolver 
    {
        [DI] private readonly MainAspect _mainAspect = default;
        public float Solve(ProtoEntity entity)
        {
            if (_mainAspect.CurrentWork.Has(entity)) 
            {
                return 10f;
            }
            
            return 0f;
        }
        public void Apply(ProtoEntity entity)
        {
            if (_mainAspect.AddSolution(_mainAspect.WorkProcess, entity, out var solutionEntity))
            {
                Debug.Log("added solution");
                
                var position = _mainAspect.Position.Get(entity).value;
                
                _mainAspect.Position.Add(solutionEntity).value = position;
                
                _mainAspect.CurrentWork.Get(entity).value.Apply(solutionEntity);
            }
        }
    }

    public interface INewWork
    {
        public bool IsDone();
        public void Apply(ProtoEntity entity);
    }
    public sealed class NewFindItemWork : INewWork
    {
        private MainAspect _mainAspect;
        public NewFindItemWork(MainAspect aspect) => _mainAspect = aspect;
        public bool IsDone()
        {
            return _mainAspect.ItemsFree.Len() > 0;
        }
        public void Apply(ProtoEntity entity)
        {
            _mainAspect.FindItemProcess.Add(entity);
            _mainAspect.FindNearElement.Add(entity).Iterator = _mainAspect.ItemsFree;
        }
    }
    
    public sealed class NewFindMineWork : INewWork
    {
        private MainAspect _mainAspect;
        public NewFindMineWork(MainAspect aspect) => _mainAspect = aspect;
        public bool IsDone()
        {
            return _mainAspect.MiningFree.Len() > 0;
        }
        public void Apply(ProtoEntity entity)
        {
            _mainAspect.MineProcess.Add(entity);
            _mainAspect.FindNearElement.Add(entity).Iterator = _mainAspect.MiningFree;
        }
    }

    public struct WorkCost
    {
        public float value;
    }
    public struct CurrentWork
    {
        public INewWork value;
    }
    public struct NewWork
    {
        public INewWork newWork;
    }
    public sealed class NewWorkSystem : IProtoRunSystem
    {
        [DI] private readonly SceneData _sceneData;
        [DI] private readonly RuntimeData _runtimeData;
        [DI] private readonly StaticData _staticData;

        [DI] private readonly MainAspect _aspect;
        public void Run()
        {
            foreach (var work in _runtimeData.Works)
            {
                if(!work.IsDone()) continue;
                
                foreach (var unitEntity in _aspect.WorkersNotWorking)
                {
                    
                    var works = _aspect.Works.Get(unitEntity).value;
                    
                    RecalculationPriority(works, work, unitEntity);
                }
            }
            
            foreach (var unitEntity in _aspect.NewWorkIt)
            {
                ref var unitNewWork = ref _aspect.NewWork.Get(unitEntity);
                
                _aspect.CurrentWork.Add(unitEntity).value = unitNewWork.newWork;
                        
                _aspect.NewWork.Del(unitEntity);
                _aspect.WorkCost.Del(unitEntity);
            }
        }

        private void RecalculationPriority(Work[] works, INewWork work, ProtoEntity unitEntity)
        {
            foreach (var unitWork in works)
            {
                if (unitWork.valueNew == work)
                {
                    if(unitWork.Order == 0) continue;
                            
                    ref var workCost = ref _aspect.WorkCost.GetOrAdd(unitEntity, out _).value;

                    if (unitWork.Order > workCost)
                    {
                        workCost = unitWork.Order;
                        
                        ref var newWork = ref _aspect.NewWork.GetOrAdd(unitEntity, out _);
                                
                        newWork.newWork = work;
                    }
                            
                    break;
                }
            }
        }
    }
    
    public sealed class WorkSystem : IProtoRunSystem
    {
        [DI] private readonly SceneData _sceneData;
        [DI] private readonly RuntimeData _runtimeData;
        [DI] private readonly StaticData _staticData;
           
        [DI] private readonly MainAspect _aspect;
        
        public void Run()
        {
            foreach (var worker in _aspect.WorkersNotWorking)
            {
                var work = _aspect.Works.Get(worker);
                foreach (var work1 in work.value)
                {
                    if (work1.value.IsDone())
                    {
    
                        work1.value.GiveWork(worker);

                        _aspect.WorkProcess.GetOrAdd(worker, out _);
                    }
                }
            }
        }
        
    }
}