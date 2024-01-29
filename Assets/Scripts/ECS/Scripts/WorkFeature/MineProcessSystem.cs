using ECS.Scripts.Boot;
using ECS.Scripts.Data;
using ECS.Scripts.GeneralComponents;
using ECS.Scripts.Path.Component;
using ECS.Scripts.TestSystem;
using Leopotam.Ecs;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace ECS.Scripts.WorkFeature
{
    public sealed class MineProcessSystem : IProtoRunSystem
    {
        [DI] private readonly SceneData _sceneData;
        [DI] private readonly RuntimeData _runtimeData;
        [DI] private readonly StaticData _staticData;
        
        
        [DI] private readonly MainAspect _mainAspect;
        public void Run()
        {
            CheckDistanceToMine();
            
            Mining();
            
            CancelMining();
        }

        private void CancelMining()
        {
            foreach (var entityProcess in _mainAspect.MiningProcessCancel)
            {
                ref readonly var owner = ref _mainAspect.Owners.Get(entityProcess).value;
                owner.Unpack(_mainAspect.World(), out var ownerEntity);
     
                Debug.Log($"Destroy");
                
                _mainAspect.CurrentWork.Del(ownerEntity);
            }
        }

        private void Mining()
        {
            foreach (var entityProcess in _mainAspect.MiningProcessMining)
            {
                ref readonly var packedEntity = ref _mainAspect.TargetWork.Get(entityProcess).PackedEntity;
                packedEntity.Unpack(_mainAspect.World(), out var miningEntity);
                
                ref var health =  ref _mainAspect.Health.Get(miningEntity).value;
                health -= 1 * _runtimeData.deltaTime;
                
                _mainAspect.OnChangeHealth.GetOrAdd(miningEntity, out _);
                
                if (health <= 0)
                {
                    Debug.Log($"Kill");
                    _mainAspect.CancelWork.Add(entityProcess);
                }
            }
        }

        private void CheckDistanceToMine()
        {
            foreach (var entityProcess in _mainAspect.MiningProcessMove)
            {
                ref readonly var owner = ref _mainAspect.Owners.Get(entityProcess).value;
                
                owner.Unpack(_mainAspect.World(), out var ownerEntity);
                
                ref readonly var position = ref _mainAspect.Position.Get(ownerEntity).value;
                
                ref readonly var packedEntity = ref _mainAspect.TargetWork.Get(entityProcess).PackedEntity;
                
                packedEntity.Unpack(_mainAspect.World(), out var miningEntity);
                
                ref readonly var positionItem = ref _mainAspect.Position.Get(miningEntity).value;
                
                var dist = position.FastDistance(positionItem);
                
                if (dist < 0.1f)
                {
                    
                    Debug.Log($"Mine");
                    _mainAspect.Mining.Add(entityProcess);
                    
                }
            }
        }
    }
}