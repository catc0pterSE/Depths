using ECS.Scripts.CharacterComponent;
using ECS.Scripts.Data;
using ECS.Scripts.GeneralComponents;
using ECS.Scripts.Path.Component;
using ECS.Scripts.Work;
using Leopotam.Ecs;
using Level;
using UnityEngine;

namespace ECS.Scripts.Path.Systems
{
    public sealed class CreateRandPathSystem : IEcsRunSystem
    {
        private readonly EcsFilter<Position, RandMove>.Exclude<Component.Path, WorkProcess> _units;
        private RuntimeData _runtimeData;

        private LevelPN _levelPn;
        public void Run()
        {
            foreach (var index in _units)
            {
                var entity = _units.GetEntity(index);
                ref var position = ref _units.Get1(index).value;
                ref var randMove = ref _units.Get2(index);

                randMove.time -= _runtimeData.deltaTime;
                
                if (randMove.time <= 0)
                {
                    randMove.time = Random.Range(1f, 2f);
                    
                    
                    var randDirection = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
                    
                    if (_levelPn.ExitBounds(position + randDirection))
                    {
                        continue;
                    }
       
                    entity.Get<TargetPath>().value = position + randDirection;
                }
            }
        }
    }
    
    public sealed class CreatePathSystem : IEcsRunSystem
    {
        private readonly EcsFilter<Position, TargetPath>.Exclude<Component.Path> _units;
		
        private readonly LevelPN _levelPN;

        public void Run()
        {
            foreach (var unitIndex in _units)
            {
                ref var entity = ref _units.GetEntity(unitIndex);
				
                ref readonly var position = ref _units.Get1(unitIndex).value;
                ref readonly var targetPosition = ref _units.Get2(unitIndex).value;
                
                var findPath = _levelPN.FindPath(position, targetPosition);
                
                ref var path = ref entity.Get<Component.Path>();
                path.value = findPath;
                path.index = 0;
                
                entity.Del<TargetPath>();
            }
        }
    }
    
    public sealed class CreateMousePathSystem : IEcsRunSystem
    {
        private readonly EcsFilter<Unit, Position, Selected> _units;
        public void Run()
        {
            if (!Input.GetMouseButtonDown(1))
            {
                return;
            }

            var cameraRay = Object.FindFirstObjectByType<CameraController>();
			
            foreach (var unitIndex in _units)
            {
                ref var entity = ref _units.GetEntity(unitIndex);
                entity.Get<TargetPath>().value = cameraRay.GetWorldPosition();
            }
        }
    }
}