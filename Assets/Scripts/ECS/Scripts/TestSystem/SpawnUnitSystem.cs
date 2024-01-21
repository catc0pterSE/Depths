using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ECS.Scripts.Boot;
using ECS.Scripts.CharacterComponent;
using ECS.Scripts.Data;
using ECS.Scripts.GeneralComponents;
using ECS.Scripts.ProviderComponents;
using ECS.Scripts.WorkFeature;
using Leopotam.Ecs;
using Level;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace ECS.Scripts.TestSystem
{
    public sealed class SpawnItemSystem : IEcsRunSystem
    {
        private readonly SceneData _sceneData;
        private readonly RuntimeData _runtimeData;
        private readonly StaticData _staticData;
        private readonly EcsWorld _world;

        public void Run()
        {
            if (!Input.GetKeyDown(KeyCode.E))
            {
                return;
            }
            int count = 100;

            while (count > 0)
            {
                var instanceObject = Object.Instantiate(_staticData.MinePrefab);

                var entityUnit = _world.NewEntity();

                entityUnit.Get<MiningTag>();
                entityUnit.Get<Health>().value = 5f;
                entityUnit.Get<TransformRef>().value = instanceObject.transform;

                var pos =  new Vector3(Random.Range(0f, 50f), Random.Range(0f, 50f));;
                pos.x = MathF.Round(pos.x);
                pos.y = MathF.Round(pos.y);
                entityUnit.Get<Position>().value = pos;

                count--;
            }
        }

        
    }
    public sealed class SpawnUnitSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsWorld _world;
		
        private readonly StaticData _staticData;

        private readonly ISelectionService _selectionService;
        
        private readonly EcsFilter<Item, Position>.Exclude<ItemPlaced> _items;
        private readonly EcsFilter<Unit> _units;
        private readonly EcsFilter<MiningTag, Position>.Exclude<ItemPlaced> _mining;
        private FindItemWork _findItemWork;
        private FindMineWork _findMineWork;

        public void Run()
        {
            if (!Input.GetKeyDown(KeyCode.Space))
            {
                return;
            }

            int count = 100;

            while (count > 0)
            {

                var instanceObject = Object.Instantiate(_staticData.UnitPrefab);
                var entityUnit = _world.NewEntity();

                entityUnit.Get<Unit>();
                entityUnit.Get<TransformRef>().value = instanceObject.transform;
                entityUnit.Get<Selected>();
                entityUnit.Get<RandMove>();
                
                var cameraRay = Object.FindFirstObjectByType<CameraController>();
                var pos = cameraRay.GetWorldPosition();
                pos.z = 0;



                entityUnit.Get<Position>().value = pos;

                CreateBody(entityUnit);

                CreateStats(entityUnit);

                var findWork = new Work();
                findWork.Order = 0;
                findWork.value = _findItemWork;

                
                var findMine = new Work();
                findMine.Order = 0;
                findMine.value = _findMineWork;
                

                entityUnit.Get<Works>().value = new Work[]
                {
                    findMine,
                    findWork
                };

                count--;
            }
            var window = Object.FindFirstObjectByType<UnitWindow>();
            
            window.SetParts($"Count:" + _units.GetEntitiesCount());

            // _selectionService.SelectUnit(entityUnit);
        }

        private void CreateStats(EcsEntity entityUnit)
        {
            ref var stats = ref entityUnit.Get<Stats>();
            stats.value = new Dictionary<StatType, EcsEntity>();

            foreach (var statData in _staticData.StatsData)
            {
                var statEntity = _world.NewEntity();
                statEntity.Get<Stat>().type = statData.Stat;
                statEntity.Get<Stat>().value = 10f;


                if (statData.Stat == StatType.Speed)
                {
                    entityUnit.Get<Speed>().value = 10f;
                }
                
                
                stats.value.Add(statData.Stat, statEntity);
            }
        }

        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        public void Init()
        {
            _findItemWork = new FindItemWork(_items);
            _findMineWork = new FindMineWork(_mining);
        }
    }
}