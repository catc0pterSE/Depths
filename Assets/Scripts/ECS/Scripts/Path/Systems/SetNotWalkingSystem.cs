using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Scripts.Path.Systems
{
    public sealed class SetNotWalkingSystem : IEcsRunSystem
    {
        private readonly LevelPN _levelPN;
        public void Run()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _levelPN.SetNotWalkingNode();
            }
        }
    }
}