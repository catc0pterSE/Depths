using ECS.Scripts.Boot;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace ECS.Scripts.PathFeature.Systems
{
    public sealed class SelectedEventSystem : IProtoRunSystem
    {
        [DI] private readonly SelectionAspect _selectionAspect;
        public void Run()
        {
            foreach (var protoEntity in _selectionAspect.SelectedEventIt)
            {
                _selectionAspect.Selected.Add(protoEntity);
                _selectionAspect.SelectedEvent.Del(protoEntity);
            }
        }
    }
}