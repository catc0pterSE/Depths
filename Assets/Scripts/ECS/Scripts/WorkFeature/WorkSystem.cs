using System;
using System.Collections.Generic;
using ECS.Scripts.CharacterComponent;
using ECS.Scripts.Data;
using ECS.Scripts.GeneralComponents;
using ECS.Scripts.Path.Component;
using ECS.Scripts.ProviderComponents;
using ECS.Scripts.TestSystem;
using Leopotam.Ecs;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ECS.Scripts.WorkFeature
{

    public sealed class MineDiedSystem : IEcsRunSystem
    {
        private readonly SceneData _sceneData;
        private readonly RuntimeData _runtimeData;
        private readonly StaticData _staticData;

        private readonly EcsWorld _world;

        private readonly EcsFilter<MiningTag, Health, Position> _filter;

        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var health = ref _filter.Get2(index).value;
                
                if(health <= 0)
                {

                    var entity = _filter.GetEntity(index);
                    entity.Del<MiningTag>();
                    entity.Get<TransformRef>().value.gameObject.SetActive(false);
                    
                    
                    ref readonly var position = ref _filter.Get3(index).value;
                    
                    var instanceObject = Object.Instantiate(_staticData.ItemPrefab);
            
                    var entityUnit = _world.NewEntity();
                    
                    entityUnit.Get<Item>();
                    entityUnit.Get<Position>().value = position;
                    entityUnit.Get<TransformRef>().value = instanceObject.transform;
                }
            }
        }

        
    }
    
    public sealed class WorkSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly SceneData _sceneData;
        private readonly RuntimeData _runtimeData;
        private readonly StaticData _staticData;

        private readonly EcsFilter<Works>.Exclude<WorkProcess> _filter;
        private readonly EcsFilter<MiningTag, Position>.Exclude<ItemPlaced> _items;
       
            
        
        private Dictionary<WorkType, Func<bool>> _conditions;
        
        public enum WorkType
        {
            FindItem,
        }

        public void Init()
        {
            _conditions = new Dictionary<WorkType, Func<bool>>()
            {
                [WorkType.FindItem] = () => !_items.IsEmpty(),
            };
        }
        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var works = ref _filter.Get1(index);
                
                foreach (var work in works.value)
                {
                    if (work.value.IsDone())
                    {
                        ref var entity = ref _filter.GetEntity(index);
                        
                        work.value.GiveWork(entity);

                        entity.Get<WorkProcess>();
                        
                        break;
                    }
                }
            }
        }
        
    }
}