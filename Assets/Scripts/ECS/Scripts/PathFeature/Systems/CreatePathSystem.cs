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
        private readonly EcsFilter<Position, TargetPoint> _units;
		
        [DI] private readonly LevelPN _levelPN;
        
        [DI] private readonly SceneData _sceneData;
        [DI] private readonly RuntimeData _runtimeData;
        [DI] private readonly StaticData _staticData;
        
        [DI] private readonly MainAspect _aspect;
        [DI] private PathAspect _pathAspect;

        public void Run()
        {
            foreach (var unitIndex in _pathAspect.PathCreate)
            {
    			
                ref readonly var position = ref _aspect.Position.Get(unitIndex).value;
                ref readonly var targetPosition = ref _pathAspect.TargetPoint.Get(unitIndex).value;
          
                if (_pathAspect.Path.Has(unitIndex))
                {
                    ref var path = ref _pathAspect.Path.Get(unitIndex);
                    
                    ListPool<Vector3>.Release(path.value);
                    
                    var findPath = _levelPN.FindPath(position, targetPosition);
                    path.value = findPath;
                    path.index = findPath.Count - 1;
                }
                else
                {

                    var findPath = _levelPN.FindPath(position, targetPosition);
                
                    ref var path = ref _pathAspect.Path.Add(unitIndex);
                    path.value = findPath;
                    path.index = findPath.Count - 1;
                }
                
                
                _pathAspect.TargetPoint.Del(unitIndex);
            }
        }
    }
}