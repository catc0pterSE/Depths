using ECS.Scripts.Boot;
using ECS.Scripts.Data;
using ECS.Scripts.GeneralComponents;
using ECS.Scripts.Path.Component;
using ECS.Scripts.TestSystem;
using Leopotam.Ecs;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace ECS.Scripts.WorkFeature
{

    public struct Speed
    {
        public float value;
    }
    public sealed class MineProcessSystem : IProtoRunSystem
    {
        [DI] private readonly SceneData _sceneData;
        [DI] private readonly RuntimeData _runtimeData;
        [DI] private readonly StaticData _staticData;
        
        
        [DI] private readonly MainAspect _aspect;
        public void Run()
        {
            foreach (var index in _aspect.MiningProcessMove)
            {
                ref readonly var component = ref _aspect.MineProcess.Get(index);
                ref readonly var position = ref _aspect.Position.Get(index).value;

                component.ItemEntity.Unpack(_aspect.World(), out var itemEntity);
                
                ref var positionItem = ref _aspect.Position.Get(itemEntity).value;

                var dist = (position - positionItem).sqrMagnitude;
                if (dist < 0.1f)
                {
                    _aspect.Mining.Add(index);
                }
            }
            
            foreach (var index in _aspect.MiningProcessMining)
            {
                ref readonly var component = ref _aspect.MineProcess.Get(index);
                
                component.ItemEntity.Unpack(_aspect.World(), out var itemEntity);
                
                ref var health =  ref _aspect.Health.Get(itemEntity).value;
                health -= 1 * _runtimeData.deltaTime;
                
                _aspect.OnChangeHealth.GetOrAdd(itemEntity, out _);
                
                if (health <= 0)
                {
                    _aspect.CancelWork.Add(index);
                }
            }
            
            foreach (var index in _aspect.MiningProcessCancel)
            {
                _aspect.MineProcess.Del(index);
                _aspect.Mining.Del(index);
            }
        }
    }
}