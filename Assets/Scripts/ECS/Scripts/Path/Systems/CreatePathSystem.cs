using ECS.Scripts.GeneralComponents;
using ECS.Scripts.Path.Component;
using Grid.Elements;
using Leopotam.Ecs;
using Level;
using UnityEngine;
using UnityEngine.Pool;

namespace ECS.Scripts.Path.Systems
{
    public sealed class CreatePathSystem : IEcsRunSystem
    {
        private readonly EcsFilter<Position, TargetPath> _units;
		
        private readonly LevelPN _levelPN;

        public void Run()
        {
            foreach (var unitIndex in _units)
            {
                ref var entity = ref _units.GetEntity(unitIndex);
				
                ref readonly var position = ref _units.Get1(unitIndex).value;
                ref readonly var targetPosition = ref _units.Get2(unitIndex).value;


                if (entity.Has<Components.Path>())
                {
                    ref var path = ref entity.Get<Components.Path>();
                    
                    ListPool<Vector3>.Release(path.value);
                    
                    var findPath = _levelPN.FindPath(position, targetPosition);
                    path.value = findPath;
                    path.index = findPath.Count - 1;
                }
                else
                {
                    var findPath = _levelPN.FindPath(position, targetPosition);
                
                    ref var path = ref entity.Get<Components.Path>();
                    path.value = findPath;
                    path.index = findPath.Count - 1;
                }
                
       
                
                entity.Del<TargetPath>();
            }
        }
    }
}