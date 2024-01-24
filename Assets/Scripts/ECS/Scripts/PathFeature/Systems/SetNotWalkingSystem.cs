using Leopotam.EcsProto;
using Level;
using UnityEngine;

namespace ECS.Scripts.PathFeature.Systems
{
    public sealed class SetNotWalkingSystem : IProtoRunSystem
    {
        private readonly PathFindingService _pathFindingService;
        public void Run()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _pathFindingService.SetNotWalkingNode();
            }
        }
    }
}