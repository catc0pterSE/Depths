using System;
using System.Collections.Generic;
using ECS.Scripts.Data;
using ECS.Scripts.GeneralComponents;
using ECS.Scripts.TestSystem;
using Leopotam.Ecs;
using Unity.VisualScripting;

namespace ECS.Scripts.Work
{
    
    
    public sealed class WorkSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly SceneData _sceneData;
        private readonly RuntimeData _runtimeData;
        private readonly StaticData _staticData;

        private readonly EcsFilter<Works>.Exclude<WorkProcess> _filter;
        private readonly EcsFilter<Item, Position>.Exclude<ItemPlaced> _items;
        
        
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