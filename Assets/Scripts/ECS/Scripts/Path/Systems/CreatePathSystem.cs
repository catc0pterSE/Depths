using ECS.Scripts.GeneralComponents;
using ECS.Scripts.Path.Component;
using Leopotam.Ecs;
using Level;
using UnityEngine;

namespace ECS.Scripts.Path.Systems
{
    public sealed class CreatePathSystem : IEcsRunSystem
    {
        private readonly EcsFilter<Position, TargetPath>.Exclude<Component.Path> _units;
		
        private readonly LevelPN _levelPN;

        public void Run()
        {
            foreach (var unitIndex in _units)
            {

                Debug.Log("Path");
                
                ref var entity = ref _units.GetEntity(unitIndex);
				
                ref readonly var position = ref _units.Get1(unitIndex).value;
                ref readonly var targetPosition = ref _units.Get2(unitIndex).value;
                
                
                Debug.Log(targetPosition);
                
                var findPath = _levelPN.FindPath(position, targetPosition);
                
                ref var path = ref entity.Get<Component.Path>();
                path.value = findPath;
                path.index = 0;
                
                entity.Del<TargetPath>();
            }
        }
    }
}