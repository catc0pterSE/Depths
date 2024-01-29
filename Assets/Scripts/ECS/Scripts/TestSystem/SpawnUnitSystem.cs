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
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Level;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace ECS.Scripts.TestSystem
{
    public sealed class SpawnUnitSystem : IProtoRunSystem, IProtoInitSystem
    {
        [DI] private readonly StaticData _staticData;

        [DI] private readonly MainAspect _aspect;
        
        [DI] private readonly StatAspect _statAspect;
        
        [DI] private readonly BodyAspect _bodyAspect;
        
        [DI] private readonly SelectionAspect _selectionAspect;

        private int _i;
        [DI] private RuntimeData _runtimeData;

        public void Run()
        {
            if (!Input.GetKeyDown(KeyCode.Space))
            {
                return;
            }

            int count = 1;

            while (count > 0)
            {

                var instanceObject = Object.Instantiate(_staticData.UnitPrefab);
                
                var cameraRay = Object.FindFirstObjectByType<CameraController>();
                var pos = cameraRay.GetWorldPosition();
                pos.z = 0;
                
                
                var entityUnit = _aspect.World().NewEntity();
                
                _aspect.Units.Add(entityUnit);
                _aspect.Transforms.Add(entityUnit).value = instanceObject.transform;
                _aspect.Position.Add(entityUnit).value = pos;


                var solutionEntity = _aspect.World().NewEntity();
                _aspect.RandMove.Add(solutionEntity);
                _aspect.Owners.Add(solutionEntity).value = _aspect.World().PackEntity(entityUnit);
                
                
                var packedSolution = _aspect.World().PackEntity(solutionEntity);
                _aspect.AISolution.Add(entityUnit).packedEntity = packedSolution;
                
                
                _selectionAspect.CanSelect.Add(entityUnit);
                _aspect.RandMove.Add(entityUnit);
                

                //CreateBody(entityUnit);

                //CreateStats(entityUnit);
                
                _aspect.Speed.Add(entityUnit).value = 10f;

                var randOrder = Random.Range(0, 2);
                
                var findWork = new Work();
                findWork.Order = randOrder == 0 ? 1 : 2;
                
                Debug.Log("1 " +  findWork.Order);
                
                findWork.valueNew = _runtimeData.Works[0];

                
                var findMine = new Work();
                findMine.Order = randOrder == 0 ? 2 : 1;
                
                Debug.Log("2 " +  findMine.Order);
                findMine.valueNew = _runtimeData.Works[1];
                

                _aspect.Works.Add(entityUnit).value = new Work[]
                {
                    findMine,
                    findWork
                };

                count--;
            }
            var window = Object.FindFirstObjectByType<UnitWindow>();
            _i++;
            window.SetParts($"Count:" + 100 * _i);
        }

        private void CreateStats(ProtoEntity entityUnit)
        {
            ref var stats = ref _statAspect.Stats.Add(entityUnit);
            stats.value = new Dictionary<StatType, ProtoPackedEntity>();


            _aspect.Speed.Add(entityUnit).value = 10f;
            
            foreach (var statData in _staticData.StatsData)
            {
                var statEntity = _aspect.World().NewEntity();


                var stat = new Stat();
                stat.type = statData.StatType;
                stat.value = 10f;
                
                _statAspect.Stat.Add(statEntity) = stat;
                
                stats.value.Add(statData.StatType, _aspect.World().PackEntity(statEntity));
            }
        }

        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CreateBody(ProtoEntity entityUnit)
        {
            ref var body = ref _bodyAspect.Bodies.Add(entityUnit);

            body.parts = new Dictionary<BodyPart, ProtoPackedEntity>();
            foreach (var bodyPartData in _staticData.BodyPartsData)
            {
                var partEntity =  _aspect.World().NewEntity();
                
                _aspect.Health.Add(partEntity).value  = 100f;
                _bodyAspect.Parts.Add(partEntity).value   = bodyPartData.Part;
                _aspect.Owners.Add(partEntity).value  = _aspect.World().PackEntity(entityUnit);

                switch (bodyPartData.Part)
                {
                    case BodyPart.Head:  _bodyAspect.Heads.Add(partEntity);
                        break;
                }
                
                body.parts.Add(bodyPartData.Part, _aspect.World().PackEntity(partEntity));
            }
        }

        public void Init(IProtoSystems systems)
        {
            _runtimeData.Works = new INewWork[]
            {
                new NewFindItemWork(_aspect),
                new NewFindMineWork(_aspect)
            };
        }
    }
}