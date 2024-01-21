using Leopotam.EcsProto;
using Level;
using UnityEngine;

namespace ECS.Scripts.PathFeature.Systems
{
    public sealed class SetNotWalkingSystem : IProtoRunSystem
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