using System.Threading.Tasks;
using ECS.Scripts.Data;
using ECS.Scripts.GeneralComponents;
using ECS.Scripts.TestSystem;
using ECS.Scripts.WorkFeature;
using Leopotam.Ecs;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Scripts.Path.Systems
{
    public sealed class PathMoveSystem : IEcsRunSystem
    {
        private readonly SceneData _sceneData;
        private readonly RuntimeData _runtimeData;
        private readonly StaticData _staticData;

        private readonly EcsFilter<Position, Components.Path> _unitsPath;
        
        private readonly EcsFilter<Position, Direction, Speed> _unitsMoveDirection;
        
        
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
        }
        
        public void Run()
        {
            if (_unitsPath.IsEmpty()) return;
            
            foreach (var index in _unitsPath)
            {
                ref var position = ref _unitsPath.Get1(index).value;
                
                ref var points = ref _unitsPath.Get2(index);
                
                ref var entity = ref _unitsPath.GetEntity(index);
                
                var cellView = points.value[points.index];
                
                var dir = cellView.WorldPosition - position;
                
                if(dir.sqrMagnitude == 0f)
                {
                    points.index++;

                    if (points.index >= points.value.Count)
                    {
                        entity.Del<Direction>();
                        entity.Del<Components.Path>();
                        
                        continue;
                    }
                }

                // entity.Get<Direction>().value = dir.normalized;
                entity.Get<Direction>().value = cellView.WorldPosition;
            }

            
            if (_unitsMoveDirection.IsEmpty()) return;
            
            var positionNative = new NativeArray<MoveData>(_unitsMoveDirection.GetEntitiesCount(), Allocator.TempJob);

            for (int i = 0; i < positionNative.Length; i++)
            {
                var MoveData = new MoveData();
                MoveData.position = _unitsMoveDirection.Get1(i).value;
                MoveData.direction = _unitsMoveDirection.Get2(i).value;
                MoveData.speed = _unitsMoveDirection.Get3(i).value;

                // Debug.Log($"{MoveData.position} {MoveData.direction} {MoveData.speed}");
                positionNative[i] = MoveData;
            }

            MoveJob moveJob = new MoveJob() { delta = _runtimeData.deltaTime, data = positionNative };
            
            moveJob.Schedule().Complete();
            
            for (int i = 0; i < positionNative.Length; i++)
            {
                _unitsMoveDirection.Get1(i).value = positionNative[i].result;
            }

            positionNative.Dispose();

            // foreach (var index in _unitsMoveDirection)
            // {
            //     ref var position = ref _unitsMoveDirection.Get1(index).value;
            //     ref var direction = ref _unitsMoveDirection.Get2(index).value;
            //     var speed = _unitsMoveDirection.Get3(index).value[StatType.Speed].Get<Stat>().TotalValue();
            //     
            //     
            //     // position += direction * (speed * _runtimeData.deltaTime);
            // }
        }
        
       
    }
}