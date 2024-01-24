using ECS.Scripts.Boot;
using ECS.Scripts.Data;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace ECS.Scripts.PathFeature.Systems
{
    public sealed class SelectedViewEventSystem : IProtoRunSystem
    {
        [DI] private readonly MainAspect _mainAspect;
        [DI] private readonly SelectionAspect _selectionAspect;
        [DI] private readonly StaticData _staticData;
        public void Run()
        {
            foreach (var protoEntity in _selectionAspect.SelectedUnitViewsOn)
            {
                Debug.Log("1");
                
                var tr = _mainAspect.Transforms.Get(protoEntity).value;
                var instance = Object.Instantiate(_staticData.PrefabSelect, tr);
                _selectionAspect.SelectedView.Add(protoEntity).value = instance.transform;
            }
        }
    }
    public sealed class SelectedEventSystem : IProtoRunSystem
    {
        [DI] private readonly SelectionAspect _selectionAspect;
        public void Run()
        {
            foreach (var protoEntity in _selectionAspect.SelectedUnitsEvent)
            {
                _selectionAspect.Selected.Add(protoEntity);
                _selectionAspect.SelectedEvent.Del(protoEntity);
            }
        }
    }
}