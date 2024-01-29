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

        [DI] private PathFindingService _pathFindingService;
        
        [DI] private readonly MainAspect _mainAspect;
        
        [DI] private PathAspect _pathAspect;
        public void Run()
        {
            foreach (var entityRand in _mainAspect.RandsMover)
            {
                ref readonly var owner = ref _mainAspect.Owners.Get(entityRand).value;
                owner.Unpack(_mainAspect.World(), out var ownerEntity);
                
                if(_mainAspect.PathAspect.Path.Has(ownerEntity))
                {
                    continue;
                }

                
                ref readonly var position = ref _mainAspect.Position.Get(ownerEntity).value;
                
                ref var randMove = ref _mainAspect.RandMove.Get(entityRand);

                randMove.time -= _runtimeData.deltaTime;
                
                if (randMove.time <= 0)
                {
                    randMove.time = Random.Range(1f, 2f);
                    var randDirection = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f));

                    var pathEnd = position + randDirection;
                    
                    if (_pathFindingService.OutBounds(pathEnd))
                    {
                        continue;
                    }

                    ref var createPath = ref _pathAspect.CreatePath.GetOrAdd(ownerEntity, out _);
                    
                    createPath.start = position;
                    createPath.end = pathEnd;
                   
                }
            }
        }
    }
}