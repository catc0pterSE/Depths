using ECS.Scripts.Boot;
using ECS.Scripts.CharacterComponent;
using ECS.Scripts.Data;
using ECS.Scripts.GeneralComponents;
using ECS.Scripts.Path.Component;
using Leopotam.Ecs;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Level;
using UnityEngine;

namespace ECS.Scripts.PathFeature.Systems
{
    public sealed class CreateMousePathSystem : IProtoRunSystem
    {
        private readonly EcsFilter<Unit, Position, CanSelect> _units;

        
        [DI] private readonly SceneData _sceneData;
        [DI] readonly RuntimeData _runtimeData;
        [DI] private readonly StaticData _staticData;
        
        [DI] private MainAspect _aspect;
        [DI] private PathAspect _pathAspect;

        [DI] private readonly PathFindingService _pathFindingService;

        
        private CameraController _controller;
        public void Run()
        {
            if (!Input.GetMouseButtonDown(1))
            {
                return;
            }

            if (_controller == null)
            {
                _controller = Object.FindFirstObjectByType<CameraController>();
            }
			
            foreach (var unitIndex in _aspect.UnitsSelected)
            {
                ref var createPath = ref _pathAspect.CreatePath.Add(unitIndex);
                createPath.start = _aspect.Position.Get(unitIndex).value;
                createPath.end = _controller.GetWorldPosition();
            }
        }
    }
}