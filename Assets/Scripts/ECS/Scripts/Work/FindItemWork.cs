using ECS.Boot;
using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Scripts.Work
{
    public class FindItemWork : IWork
    {
        private readonly EcsFilter<Item, Position>.Exclude<ItemInHand> _items;

        public FindItemWork(EcsFilter<Item, Position>.Exclude<ItemInHand> items)
        {
            _items = items;
        }
        public bool IsDone()
        {
            return !_items.IsEmpty();
        }
        public void GiveWork(EcsEntity entity)
        {
            ref readonly var position = ref entity.Get<Position>().value;
            var distance = Mathf.Infinity;
            Vector3 positionTarget = Vector3.zero;
            EcsEntity findEntity = default; 
            
            foreach (var indexItem in _items)
            {
                ref readonly var positionItem = ref _items.Get2(indexItem).value;
                
                var distanceTwo = (positionItem - position).sqrMagnitude;

                if (distance < distanceTwo)
                {
                    positionTarget = positionItem;
                    distance = distanceTwo;
                    findEntity = _items.GetEntity(indexItem);
                }
            }
            
            entity.Get<FindItemProcess>().ItemEntity = findEntity;
            entity.Get<TargetPath>().value = positionTarget;
        }
    }
}