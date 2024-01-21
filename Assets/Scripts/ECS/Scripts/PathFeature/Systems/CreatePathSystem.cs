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
        private readonly EcsFilter<Position, TargetPath> _units;
		
        [DI] private readonly LevelPN _levelPN;
        
        [DI] private readonly SceneData _sceneData;
        [DI] private readonly RuntimeData _runtimeData;
        [DI] private readonly StaticData _staticData;
        
        [DI] private readonly MainAspect _aspect;

        public void Run()
        {
            foreach (var unitIndex in _aspect.PathCreate)
            {
    			
                ref readonly var position = ref _aspect.Position.Get(unitIndex).value;
                ref readonly var targetPosition = ref _aspect.TargetPath.Get(unitIndex).value;
          
                if (_aspect.Path.Has(unitIndex))
                {
                    ref var path = ref _aspect.Path.Get(unitIndex);
                    
                    ListPool<Vector3>.Release(path.value);
                    
                    var findPath = _levelPN.FindPath(position, targetPosition);
                    path.value = findPath;
                    path.index = findPath.Count - 1;
                }
                else
                {

                    var findPath = _levelPN.FindPath(position, targetPosition);
                
                    ref var path = ref _aspect.Path.Add(unitIndex);
                    path.value = findPath;
                    path.index = findPath.Count - 1;
                }
                
                
                _aspect.TargetPath.Del(unitIndex);
            }
        }
    }
}