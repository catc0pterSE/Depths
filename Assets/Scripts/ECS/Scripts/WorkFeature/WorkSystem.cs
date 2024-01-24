using System;
using System.Collections.Generic;
using ECS.Scripts.Boot;
using ECS.Scripts.CharacterComponent;
using ECS.Scripts.Data;
using ECS.Scripts.GeneralComponents;
using ECS.Scripts.Path.Component;
using ECS.Scripts.ProviderComponents;
using ECS.Scripts.TestSystem;
using Leopotam.Ecs;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
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

        private readonly EcsFilter<MiningTag, Health, Position> _filter;

        public void Run()
        {
            foreach (var index in _aspect.MiningDied)
            {
                ref var health = ref _aspect.Health.Get(index).value;
                
                if(health <= 0)
                {
                    _aspect.MiningTag.Del(index);
                    _aspect.Transforms.Get(index).value.gameObject.SetActive(false);
                    
                    
                    ref readonly var position = ref _aspect.Position.Get(index).value;
                    
                    var instanceObject = Object.Instantiate(_staticData.ItemPrefab);
            
                    var entityUnit = _aspect.World().NewEntity();
                    
                    _aspect.Items.Add(entityUnit);
                    _aspect.Position.Add(entityUnit).value = position;
                    _aspect.Transforms.Add(entityUnit).value = instanceObject.transform;
                }
            }
        }

        
    }
    
    public sealed class FindTaskSystem<T> : IProtoRunSystem
    {

        #region JOBS
        
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
                        distanceOut.indexEntity = element.indexEntity;

                        DistanceOut[0] = distanceOut;
                    }
                    
                }
            }
        }
        public struct DistanceData
        {
            public float3 PositionItem;
            public ProtoEntity indexEntity;
        }
        
        #endregion

        [DI] private readonly MainAspect _mainAspect;
        public void Run()
        {
            foreach (var entity in _mainAspect.FindWorkF)
            {
                ref readonly var position = ref _mainAspect.Position.Get(entity).value;
                
                ref readonly var data = ref _mainAspect.FindWork.Get(entity);
                
                var positionNative = new NativeArray<DistanceData>(data.items.Len(), Allocator.TempJob);
                var positionNativeOut = new NativeArray<DistanceData>(1, Allocator.TempJob);

                var distanceDataLoad = new DistanceData();
                distanceDataLoad.indexEntity = entity;
                positionNativeOut[0] = distanceDataLoad;

                int index = 0;
                foreach (var itemEntity in data.items)
                {
                    var distanceData = new DistanceData();
                    distanceData.PositionItem = _mainAspect.Position.Get(itemEntity).value;
                    distanceData.indexEntity = itemEntity;
                    positionNative[index] = distanceData;

                    index++;
                }
            
                DistanceJob moveJob = new DistanceJob { Position = position, DistanceOut = positionNativeOut, Distance = positionNative, localDistance = 100000f};

                var jobHandle = moveJob.Schedule();
            
                jobHandle.Complete();
            
                var findEntity = positionNativeOut[0].indexEntity;
            
                _mainAspect.ItemsBusy.Add(findEntity);
                _mainAspect.ItemWork.Add(entity).ItemEntity = _mainAspect.World().PackEntity(findEntity);
                
              
                ref var createPath = ref _mainAspect.PathAspect.CreatePath.GetOrAdd(entity, out _);
            
                createPath.start = position;
                createPath.end = positionNativeOut[0].PositionItem;
                
                positionNative.Dispose();
                positionNativeOut.Dispose();
                
                _mainAspect.FindWork.Del(entity);
            }
            
        }
    }

    public sealed class WorkSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] private readonly SceneData _sceneData;
        [DI] private readonly RuntimeData _runtimeData;
        [DI] private readonly StaticData _staticData;
           
        [DI] private readonly MainAspect _aspect;
        
        private Dictionary<WorkType, Func<bool>> _conditions;
        
        public enum WorkType
        {
            FindItem,
            Mining,
        }
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

                        _aspect.WorkProcess.Add(worker);
                    }
                }
            }
        }

        public void Init(IProtoSystems systems)
        {
            _conditions = new Dictionary<WorkType, Func<bool>>()
            {
                [WorkType.FindItem] = () => _aspect.ItemsFree.Len() > 0,
                [WorkType.Mining] = () => _aspect.MiningFree.Len() > 0,
            };
        }
    }
}