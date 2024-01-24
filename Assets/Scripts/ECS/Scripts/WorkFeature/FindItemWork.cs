using ECS.Scripts.Boot;
using ECS.Scripts.GeneralComponents;
using ECS.Scripts.Path.Component;
using ECS.Scripts.TestSystem;
using Leopotam.Ecs;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Scripts.WorkFeature
{
    public class FindMineWork : IWork
    {
        private readonly EcsFilter<MiningTag, Position>.Exclude<ItemBusy> _items;
        private readonly MainAspect _aspect;
        
                
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
        public FindMineWork(MainAspect aspect)
        {
            _aspect = aspect;
        }
        public bool IsDone()
        {
            return _aspect.MiningFree.Len() != 0;
        }
        public void GiveWork(ProtoEntity entity)
        {
            Debug.Log("Mine");
            
            ref readonly var position = ref _aspect.Position.Get(entity).value;
            
            var positionNative = new NativeArray<DistanceData>(_aspect.MiningFree.Len(), Allocator.TempJob);
            var positionNativeOut = new NativeArray<DistanceData>(1, Allocator.TempJob);

            var distanceDataLoad = new DistanceData();
            distanceDataLoad.indexEntity = entity;
            positionNativeOut[0] = distanceDataLoad;

            int index = 0;
            foreach (var protoEntity in _aspect.MiningFree)
            {
                var distanceData = new DistanceData();
                distanceData.PositionItem = _aspect.Position.Get(protoEntity).value;
                distanceData.indexEntity = protoEntity;
                positionNative[index] = distanceData;

                index++;
            }


            DistanceJob moveJob = new DistanceJob { Position = position, DistanceOut = positionNativeOut, Distance = positionNative, localDistance = 100000f};

            var jobHandle = moveJob.Schedule();
            
            jobHandle.Complete();
            
            var findEntity = positionNativeOut[0].indexEntity;
            
            _aspect.ItemsBusy.Add(findEntity);
            _aspect.MineProcess.Add(entity).ItemEntity = _aspect.World().PackEntity(findEntity);
            
            ref var createPath = ref _aspect.PathAspect.CreatePath.GetOrAdd(entity, out _);
            
            createPath.start = position;
            createPath.end = positionNativeOut[0].PositionItem;
            

            positionNative.Dispose();
            positionNativeOut.Dispose();
        }
    }
    public class FindItemWork : IWork
    {
        private readonly MainAspect _aspect;

      
              
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
        public FindItemWork(MainAspect aspect)
        {
            _aspect = aspect;
        }
        public bool IsDone()
        {
            return _aspect.ItemsFree.Len() != 0;
        }
        public void GiveWork(ProtoEntity entity)
        {
            ref readonly var position = ref _aspect.Position.Get(entity).value;
            
            var positionNative = new NativeArray<DistanceData>(_aspect.ItemsFree.Len(), Allocator.TempJob);
            var positionNativeOut = new NativeArray<DistanceData>(1, Allocator.TempJob);

            var distanceDataLoad = new DistanceData();
            distanceDataLoad.indexEntity = entity;
            positionNativeOut[0] = distanceDataLoad;

            int index = 0;
            foreach (var protoEntity in _aspect.ItemsFree)
            {
                var distanceData = new DistanceData();
                distanceData.PositionItem = _aspect.Position.Get(protoEntity).value;
                distanceData.indexEntity = protoEntity;
                positionNative[index] = distanceData;

                index++;
            }


            DistanceJob moveJob = new DistanceJob { Position = position, DistanceOut = positionNativeOut, Distance = positionNative, localDistance = 100000f};

            var jobHandle = moveJob.Schedule();
            
            jobHandle.Complete();
            
            var findEntity = positionNativeOut[0].indexEntity;
            
            _aspect.ItemsBusy.Add(findEntity);
            _aspect.FindItemProcess.Add(entity).ItemEntity = _aspect.World().PackEntity(findEntity);

            ref var createPath = ref _aspect.PathAspect.CreatePath.GetOrAdd(entity, out _);
            
            createPath.start = position;
            createPath.end = positionNativeOut[0].PositionItem;

            positionNative.Dispose();
            positionNativeOut.Dispose();
        }
    }
}