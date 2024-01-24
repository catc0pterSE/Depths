using ECS.Scripts.Boot;
using ECS.Scripts.Data;
using ECS.Scripts.GeneralComponents;
using ECS.Scripts.Path.Component;
using Leopotam.Ecs;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Level;
using UnityEngine;
using UnityEngine.Pool;

namespace ECS.Scripts.PathFeature.Systems
{
    public sealed class CreatePathSystem : IProtoRunSystem
    {
        private readonly EcsFilter<Position, CreatePath> _units;
		
        [DI] private readonly PathFindingService _pathFindingService;
        
        [DI] private readonly SceneData _sceneData;
        [DI] private readonly RuntimeData _runtimeData;
        [DI] private readonly StaticData _staticData;
        
        [DI] private readonly MainAspect _aspect;
        [DI] private PathAspect _pathAspect;

        public void Run()
        {
            foreach (var unitIndex in _pathAspect.CreatePathIt)
            {
                var packedEntity = _aspect.World().PackEntity(unitIndex);
                
                ref readonly var createPath = ref _pathAspect.CreatePath.Get(unitIndex);
          
                if (_pathAspect.Path.Has(unitIndex))
                {
                    ref var path = ref _pathAspect.Path.Get(unitIndex);
                    
                    ListPool<Vector3>.Release(path.value);
                    
                    var findPath = _pathFindingService.FindPath(createPath.start, createPath.end);
                    path.value = findPath;
                    path.index = findPath.Count - 1;
                }
                else
                {

                    var findPath = _pathFindingService.FindPath(createPath.start, createPath.end);
                
                    ref var path = ref _pathAspect.Path.Add(unitIndex);
                    path.value = findPath;
                    path.index = findPath.Count - 1;
                }
                
                
                _pathAspect.CreatePath.Del(unitIndex);
            }
        }
    }
}