using ECS.Scripts.Boot;
using ECS.Scripts.Data;
using ECS.Scripts.GeneralComponents;
using ECS.Scripts.Path.Component;
using ECS.Scripts.ProviderComponents;
using ECS.Scripts.TestSystem;
using Leopotam.Ecs;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace ECS.Scripts.WorkFeature
{
    public sealed class FindItemProcessSystem : IProtoRunSystem
    {
        [DI] private readonly SceneData _sceneData;
        [DI] private readonly RuntimeData _runtimeData;
        [DI] private readonly StaticData _staticData;
        
        
        [DI] private readonly MainAspect _aspect;

        public void Run()
        {
            foreach (var index in _aspect.FindItemProcessGet)
            {
                ref readonly var component = ref _aspect.FindItemProcess.Get(index);
                ref readonly var position = ref _aspect.Position.Get(index).value;

                component.ItemEntity.Unpack(_aspect.World(), out var itemEntity);
                ref var positionItem = ref _aspect.Position.Get(itemEntity).value;

                var dist = (position - positionItem).sqrMagnitude;

                if (dist < 0.1f)
                {
                    _aspect.ItemsInHand.Add(index).value = component.ItemEntity;   
                    
                    var trItem = _aspect.Transforms.Get(itemEntity).value;

                    _aspect.Sync.Add(itemEntity);

                    var trUnit = _aspect.Transforms.Get(index).value;;

                    trItem.SetParent(trUnit);
                    
                    _aspect.TargetPath.GetOrAdd(index, out _).value = new Vector3(5f, 5f);
                    _aspect.TargetDrop.GetOrAdd(index, out _).value = new Vector3(5f, 5f);

                }
            }

            foreach (var index in _aspect.FindItemProcessDrop)
            {

                ref var position = ref _aspect.Position.Get(index).value;;
                ref var targetDrop = ref _aspect.TargetDrop.Get(index).value;;

                var dist = position.FastDistance(targetDrop);

                if (dist < 0.01f)
                {
                    // ref var item = ref _aspect.ItemsInHand.Get(index);;
                    // item.value.Unpack(_aspect.World(), out var itemEntity);

                    _aspect.CancelWork.Add(index);
                }

            }

            foreach (var index in _aspect.FindItemProcessCancel)
            {
                if (_aspect.TargetDrop.Has(index))
                {
                    _aspect.TargetDrop.Del(index);
                }
                
                if (_aspect.ItemsInHand.Has(index))
                {
                    ref var item = ref _aspect.ItemsInHand.Get(index);;
                    
                    item.value.Unpack(_aspect.World(), out var itemEntity);
                    
                    _aspect.Transforms.Get(itemEntity).value.SetParent(null);
                    
                    _aspect.Position.Get(itemEntity).value = _aspect.Position.Get(index).value;
                    
                    _aspect.Sync.Del(itemEntity);
                    
                    _aspect.ItemsInHand.Del(index);
                }

                _aspect.FindItemProcess.Del(index);
            }
        }
    }
}