using ECS.Scripts.Boot;
using ECS.Scripts.Data;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Level;
using UnityEngine;

namespace ECS.Scripts.PathFeature.Systems
{
    public sealed class CreateRandPathSystem : IProtoRunSystem
    {
        [DI] private RuntimeData _runtimeData;

        [DI] private LevelPN _levelPn;
        
        [DI] private readonly MainAspect _aspect;
        
        [DI] private PathAspect _pathAspect;
        public void Run()
        {
            foreach (var index in _aspect.RandsMover)
            {
                ref var position = ref _aspect.Position.Get(index).value;
                ref var randMove = ref _aspect.RandMove.Get(index);

                randMove.time -= _runtimeData.deltaTime;
                
                if (randMove.time <= 0)
                {
                    randMove.time = Random.Range(1f, 2f);
                    var randDirection = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
                    
                    if (_levelPn.OutBounds(position + randDirection))
                    {
                        continue;
                    }

                    _pathAspect.TargetPoint.GetOrAdd(index, out _).value = position + randDirection;
                   
                }
            }
        }
    }
}