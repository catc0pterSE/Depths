using ECS.Scripts.Boot;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace ECS.Scripts.WorkFeature
{
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
}