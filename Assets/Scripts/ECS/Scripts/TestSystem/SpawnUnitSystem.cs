using System;
using System.Collections.Generic;
using ECS.Scripts.Work;
using Leopotam.Ecs;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace ECS.Boot
{
    public sealed class SpawnUnitSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
		
        private readonly StaticData _staticData;

        private readonly ISelectionService _selectionService;
        
        private readonly EcsFilter<Item, Position>.Exclude<ItemInHand> _items;
        
        public void Run()
        {
            if (!Input.GetKeyDown(KeyCode.Space))
            {
                return;
            }
            
            var instanceObject = Object.Instantiate(_staticData.UnitPrefab);
            
            var entityUnit = _world.NewEntity();
            
            entityUnit.Get<Unit>();
            entityUnit.Get<TransformRef>().value = instanceObject.transform;
            entityUnit.Get<Selected>();
            entityUnit.Get<RandMove>();
            entityUnit.Get<Position>().value = new Vector3(0, 1f, 0);

            CreateBody(entityUnit);

            CreateStats(entityUnit);
            
            var findWork = new Work();
            findWork.Order = 0;
            findWork.value = new FindItemWork(_items);
                
            
            entityUnit.Get<Works>().value = new Work[]
            {
                findWork
            };
            
            
            _selectionService.SelectUnit(entityUnit);
        }

        private void CreateStats(EcsEntity entityUnit)
        {
            ref var stats = ref entityUnit.Get<Stats>();
            stats.value = new Dictionary<StatType, EcsEntity>();

            foreach (var statData in _staticData.StatsData)
            {
                var statEntity = _world.NewEntity();
                statEntity.Get<Stat>().type = statData.Stat;
                statEntity.Get<Stat>().value = Random.Range(1f, 10f);
                stats.value.Add(statData.Stat, statEntity);
            }
        }

        private void CreateBody(EcsEntity entityUnit)
        {
            ref var body = ref entityUnit.Get<Body>();
            
            body.parts = new Dictionary<BodyPart, EcsEntity>();
            foreach (var bodyPartData in _staticData.BodyPartsData)
            {
                var partEntity = _world.NewEntity();
                
                partEntity.Get<Health>().value  = 100f;
                partEntity.Get<Part>().value    = bodyPartData.Part;
                partEntity.Get<Owner>().value   = entityUnit;

                switch (bodyPartData.Part)
                {
                    case BodyPart.Head: partEntity.Get<Head>();
                        break;
                }
                
                body.parts.Add(bodyPartData.Part, partEntity);
            }
        }
    }
}