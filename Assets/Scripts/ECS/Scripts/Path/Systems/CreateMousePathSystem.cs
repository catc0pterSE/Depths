using ECS.Scripts.CharacterComponent;
using ECS.Scripts.GeneralComponents;
using ECS.Scripts.Path.Component;
using Leopotam.Ecs;
using Level;
using UnityEngine;

namespace ECS.Scripts.Path.Systems
{
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