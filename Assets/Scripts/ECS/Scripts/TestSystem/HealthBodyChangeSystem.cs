using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Scripts.TestSystem
{
    public sealed class BodyStateReactionSystem : IEcsRunSystem
    {
        private readonly EcsFilter<Head, Health, OnChangeHealth> _head;
        private readonly EcsFilter<Part, Health, OnChangeHealth> _parts;
        public void Run()
        {
            foreach (var i in _head)
            {
                ref var entity = ref _head.GetEntity(i);
                ref var health = ref _head.Get2(i).value;

                if (health <= 0)
                {
                    entity.Get<Owner>().value.Get<DiedEvent>();
                }
            }
            
            foreach (var i in _parts)
            {
                ref var part = ref _parts.Get1(i).value;
                ref var health = ref _parts.Get2(i).value;
                Debug.Log($"{part.ToString()} : {health}");
            }
        }
    }
}