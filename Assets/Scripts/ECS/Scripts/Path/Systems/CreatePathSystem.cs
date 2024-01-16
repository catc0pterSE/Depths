using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Boot
{
    public sealed class CreatePathSystem : IEcsRunSystem
    {
        private readonly EcsFilter<Unit, Position, Selected> _units;
		
        private readonly LevelPN _levelPN;

        public void Run()
        {
            if (!Input.GetMouseButtonDown(1))
            {
                return;
            }

            var cameraRay = Object.FindFirstObjectByType<CameraRay>();
			
            foreach (var unitIndex in _units)
            {
                ref var entity = ref _units.GetEntity(unitIndex);
				
                ref readonly var position = ref _units.Get2(unitIndex).value;

                Physics.Raycast(position, Vector3.down, out var hit);

                var cellPosition = hit.collider.GetComponent<Cell>().Position;
                
                var findPath = _levelPN.FindPath(cellPosition, cameraRay.GetNode().Position);
                
                ref var path = ref entity.Get<Path>();
                path.value = findPath;
                path.index = 0;
            }
        }
    }
}