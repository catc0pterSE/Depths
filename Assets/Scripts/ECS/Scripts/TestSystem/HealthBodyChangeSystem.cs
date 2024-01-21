using ECS.Scripts.Boot;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace ECS.Scripts.TestSystem
{
    public sealed class BodyStateReactionSystem : IProtoRunSystem
    {
        [DI] private readonly MainAspect _aspect;
        public void Run()
        {
            foreach (var i in _aspect.HeadsChangeHealth)
            {
                ref var health = ref _aspect.Health.Get(i).value;

                if (health <= 0)
                {
                    _aspect.DiedsEvent.Add(i);
                }
            }
            
            foreach (var i in _aspect.PartsChangeHealth)
            {
                ref var part = ref _aspect.Parts.Get(i).value;
                ref var health = ref _aspect.Health.Get(i).value;
                
                Debug.Log($"{part.ToString()} : {health}");
            }
        }
    }
}