using ECS.Scripts.Boot;
using ECS.Scripts.Data;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Level;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Scripts.PathFeature.Systems
{
    public sealed class PathMoveSystem : IProtoRunSystem
    {
        [DI] private readonly SceneData _sceneData;
        [DI] readonly RuntimeData _runtimeData;
        [DI] private readonly StaticData _staticData;
        
        [DI] private readonly SpatialHash _spatialHash;

        
        [DI] private MainAspect _aspect;

        [DI] private readonly PathFindingService _pathFindingService;

        #region Jobs
        [BurstCompile]
        public struct MoveJob : IJob
        {
            public float delta;

            public NativeArray<MoveData> data;
            [BurstCompile]
            public void Execute()
            {

                for (int i = 0; i < data.Length; i++)
                {
                    var moveData = data[i]; 
                    
                    moveData.result = moveData.position.MoveTowards(moveData.direction, moveData.speed * delta);
                    
                    data[i] = moveData;
                }
                
            }
        }
        
        public struct MoveData
        {
            public float speed;
            public float3 position;
            public float3 direction;
            public float3 result;
            public ProtoEntity Id;
        }
        
        #endregion
        
        public void Run()
        {
            var countEntities = _aspect.MoveDirection.Len();

            if (countEntities == 0)
            {
                return;
            }
            
            
            var positionNative = new NativeArray<MoveData>(countEntities, Allocator.TempJob);
            
            int count = 0;
            
            foreach (var index in _aspect.MoveDirection)
            {
                var MoveData = new MoveData();
                
                MoveData.Id = index;
                MoveData.position = _aspect.Position.Get(index).value;
                MoveData.direction = _aspect.Direction.Get(index).value;
                MoveData.speed = _aspect.Speed.Get(index).value;
                positionNative[count] = MoveData;
                
                count++;
            }
            
            MoveJob moveJob = new MoveJob() { delta = _runtimeData.deltaTime, data = positionNative };
            
            moveJob.Schedule().Complete();

            var world = _aspect.World();
            for (int i = 0; i < positionNative.Length; i++)
            {
                var moveData = positionNative[i];
                _aspect.Position.Get(moveData.Id).value = moveData.result;

                var packedEntity = world.PackEntity(moveData.Id);
                
                _spatialHash.UpdatePosition(packedEntity, moveData.result);
            }

            positionNative.Dispose();
            
        }
        
       
    }
}