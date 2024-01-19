using ECS.Scripts.Data;
using ECS.Scripts.GeneralComponents;
using ECS.Scripts.Path.Component;
using ECS.Scripts.ProviderComponents;
using ECS.Scripts.TestSystem;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Scripts.WorkFeature
{

    public struct Speed
    {
        public float value;
    }
    public sealed class MineProcessSystem : IEcsRunSystem
    {
        private readonly SceneData _sceneData;
        private readonly RuntimeData _runtimeData;
        private readonly StaticData _staticData;

        private readonly EcsFilter<MineProcess, Position>.Exclude<Mining> _filter;
        
        private readonly EcsFilter<MineProcess, Position, Mining> _mining;
        
        private readonly EcsFilter<MineProcess, CancelWork> _cancel;
        public void Run()
        {
            foreach (var index in _filter)
            {
                ref readonly var component = ref _filter.Get1(index);
                ref readonly var position = ref _filter.Get2(index).value;
                
                ref var positionItem = ref component.ItemEntity.Get<Position>().value;

                var dist = (position - positionItem).sqrMagnitude;
                var entity = _filter.GetEntity(index);
                if (dist < 0.1f)
                {
                    entity.Get<Mining>();
                }
            }
            
            
            foreach (var index in _mining)
            {
                ref readonly var component = ref _mining.Get1(index);
                ref readonly var position = ref _mining.Get2(index).value;
                
                ref var positionItem = ref component.ItemEntity.Get<Position>().value;

                var dist = (position - positionItem).sqrMagnitude;
                var entity = _mining.GetEntity(index);
                

                if (dist > 0.2f)
                {
                    entity.Del<Mining>();
                    entity.Get<TargetPath>().value = positionItem;
                }
                else
                {
                    ref var health = ref component.ItemEntity.Get<Health>().value;
                    component.ItemEntity.Get<Health>().value -= 1 * _runtimeData.deltaTime;
                    component.ItemEntity.Get<Speed>().value = 4f;
                    component.ItemEntity.Get<RandMove>();
                    component.ItemEntity.Get<OnChangeHealth>();
                    if (health <= 0)
                    {
                        entity.Get<CancelWork>();
                    }
                }
            }
            
            
            
            foreach (var index in _cancel)
            {
                ref var entity = ref _cancel.GetEntity(index);
                entity.Del<MineProcess>();
                entity.Del<Mining>();
            }
        }
    }
}