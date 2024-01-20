using ECS.Scripts.GeneralComponents;
using ECS.Scripts.Path.Component;
using ECS.Scripts.Path.Systems;
using ECS.Scripts.TestSystem;
using Leopotam.Ecs;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Scripts.WorkFeature
{
    public class FindMineWork : IWork
    {
        private readonly EcsFilter<MiningTag, Position>.Exclude<ItemPlaced> _items;
        
        [BurstCompile]
        private struct DistanceJob : IJob
        {
            public NativeArray<DistanceData> Distance;
            public NativeArray<DistanceData> DistanceOut;
            
            [ReadOnly]  public float3 Position;
            [BurstCompile]
            public void Execute()
            {
                for (int i = 0; i < Distance.Length; i++)
                {
                    var element = Distance[i];
                    var distanceTwo =  math.distance(element.PositionItem, Position);
                    
                    var distanceOut = DistanceOut[0];
                    
                    if (distanceOut.distance > distanceTwo)
                    {
                        distanceOut.distance = distanceTwo;
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
            public float distance;
            public int indexEntity;
        }

        public FindMineWork(EcsFilter<MiningTag, Position>.Exclude<ItemPlaced> items)
        {
            _items = items;
        }
        public bool IsDone()
        {
            return !_items.IsEmpty();
        }
        public void GiveWork(EcsEntity entity)
        {
            // ref readonly var position = ref entity.Get<Position>().value;
            //
            // var positionNative = new NativeArray<DistanceData>(_items.GetEntitiesCount(), Allocator.TempJob);
            // var positionNativeOut = new NativeArray<DistanceData>(1, Allocator.TempJob);
            //
            // var distanceDataLoad = new DistanceData();
            // distanceDataLoad.indexEntity = 0;
            // distanceDataLoad.distance =  100000f;
            //
            // positionNativeOut[0] = distanceDataLoad;
            //
            //
            // for (int i = 0; i < positionNative.Length; i++)
            // {
            //     var distanceData = new DistanceData();
            //     distanceData.PositionItem = _items.Get2(i).value;
            //     distanceData.indexEntity = i;
            //     positionNative[i] = distanceData;
            // }
            //
            // DistanceJob moveJob = new DistanceJob { Position = position, DistanceOut = positionNativeOut, Distance = positionNative};
            //
            // var jobHandle = moveJob.Schedule();
            //
            // jobHandle.Complete();
            //
            // var findEntity = _items.GetEntity( positionNativeOut[0].indexEntity);
            //

            var itemEntity = _items.GetEntity(0);
            itemEntity.Get<ItemPlaced>();
            entity.Get<MineProcess>().ItemEntity = itemEntity;
            entity.Get<TargetPath>().value = itemEntity.Get<Position>().value;

            // positionNative.Dispose();
            // positionNativeOut.Dispose();
        }
    }
    public class FindItemWork : IWork
    {
        private readonly EcsFilter<Item, Position>.Exclude<ItemPlaced> _items;

      
              
        [BurstCompile]
        private struct DistanceJob : IJob
        {
            public NativeArray<DistanceData> Distance;
            public NativeArray<DistanceData> DistanceOut;
            
            [ReadOnly]  public float3 Position;
            [BurstCompile]
            public void Execute()
            {
                for (int i = 0; i < Distance.Length; i++)
                {
                    var element = Distance[i];
                    var distanceTwo =  math.distance(element.PositionItem, Position);
                    
                    var distanceOut = DistanceOut[0];
                    
                    if (distanceOut.distance > distanceTwo)
                    {
                        distanceOut.distance = distanceTwo;
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
            public float distance;
            public int indexEntity;
        }
        public FindItemWork(EcsFilter<Item, Position>.Exclude<ItemPlaced> items)
        {
            _items = items;
        }
        public bool IsDone()
        {
            return !_items.IsEmpty();
        }
        public void GiveWork(EcsEntity entity)
        {
            ref readonly var position = ref entity.Get<Position>().value;
   
            var positionNative = new NativeArray<DistanceData>(_items.GetEntitiesCount(), Allocator.TempJob);
            var positionNativeOut = new NativeArray<DistanceData>(1, Allocator.TempJob);

            var distanceDataLoad = new DistanceData();
            distanceDataLoad.indexEntity = 0;
            distanceDataLoad.distance =  100000f;
            
            positionNativeOut[0] = distanceDataLoad;
            
            
            for (int i = 0; i < positionNative.Length; i++)
            {
                var distanceData = new DistanceData();
                distanceData.PositionItem = _items.Get2(i).value;
                distanceData.indexEntity = i;
                positionNative[i] = distanceData;
            }

            DistanceJob moveJob = new DistanceJob { Position = position, DistanceOut = positionNativeOut, Distance = positionNative};

            var jobHandle = moveJob.Schedule();
            
            jobHandle.Complete();
            
            var findEntity = _items.GetEntity( positionNativeOut[0].indexEntity);
            
            findEntity.Get<ItemPlaced>();
            entity.Get<FindItemProcess>().ItemEntity = findEntity;
            entity.Get<TargetPath>().value = positionNativeOut[0].PositionItem;

            positionNative.Dispose();
            positionNativeOut.Dispose();
            
        }
    }
}