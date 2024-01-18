using ECS.Scripts.Data;
using ECS.Scripts.GeneralComponents;
using ECS.Scripts.Path.Component;
using ECS.Scripts.ProviderComponents;
using ECS.Scripts.TestSystem;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Scripts.Work
{
    public sealed class WorkCancelSystem : IEcsRunSystem
    {
        private readonly SceneData _sceneData;
        private readonly RuntimeData _runtimeData;
        private readonly StaticData _staticData;

        private readonly EcsFilter<WorkProcess, CancelWork> _filter;

        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var entity = ref _filter.GetEntity(index);
                entity.Del<WorkProcess>();
                entity.Del<CancelWork>();
            }
        }

        
    }
    public sealed class FindItemProcessSystem : IEcsRunSystem
    {
        private readonly SceneData _sceneData;
        private readonly RuntimeData _runtimeData;
        private readonly StaticData _staticData;

        private readonly EcsFilter<FindItemProcess, Position, TransformRef>.Exclude<ItemInHand> _filter;
        private readonly EcsFilter<FindItemProcess, CancelWork> _cancel;

        public void Check(ref EcsEntity entity)
        {
            
        }
        public void Run()
        {
            foreach (var index in _filter)
            {
                ref readonly var component = ref _filter.Get1(index);
                ref readonly var position = ref _filter.Get2(index).value;

                ref var positionItem = ref component.ItemEntity.Get<Position>().value;

                var dist = position.FastDistance(positionItem);
                
                if (dist < 0.1f)
                {
                    ref var entity = ref _filter.GetEntity(index);

                    entity.Get<ItemInHand>().value = component.ItemEntity;
                    
                    var trItem = component.ItemEntity.Get<TransformRef>().value;
                    component.ItemEntity.Get<Sync>();

                    var trUnit = _filter.Get3(index).value;
                    
                    trItem.SetParent(trUnit);

                    entity.Get<TargetPath>().value = Vector3.zero;


                    var newEntity = new EcsEntity();
                    
                    Check(ref newEntity);
                }
            }
            
            
            foreach (var index in _filter)
            {
                ref readonly var component = ref _filter.Get1(index);
                ref readonly var position = ref _filter.Get2(index).value;

                ref var positionItem = ref component.ItemEntity.Get<Position>().value;

                var dist = position.FastDistance(positionItem);
                
                if (dist < 0.1f)
                {
                    ref var entity = ref _filter.GetEntity(index);

                    entity.Get<ItemInHand>().value = component.ItemEntity;
                    
                    var trItem = component.ItemEntity.Get<TransformRef>().value;
                    component.ItemEntity.Get<Sync>();

                    var trUnit = _filter.Get3(index).value;
                    
                    trItem.SetParent(trUnit);
                }
            }
            
            

            foreach (var index in _cancel)
            {
                ref var entity = ref _cancel.GetEntity(index);
                ref var component = ref _cancel.Get1(index);
                ref var entityItem = ref component.ItemEntity;

                if (entity.Has<ItemInHand>())
                {
                    entityItem.Get<TransformRef>().value.SetParent(null);
                    entityItem.Get<Position>().value = entity.Get<Position>().value;
                    entityItem.Del<Sync>();
                }

                entity.Del<ItemInHand>();
            }
        }

        
    }
}