using ECS.Scripts.Boot;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace ECS.Scripts.SyncSystems
{

    public sealed class UpdatePositionSystem : IProtoRunSystem
    {
        [DI] private MainAspect _aspect;
        public void Run()
        {
            foreach (var index in _aspect.SyncPosition)
            {
                ref readonly var transform = ref _aspect.Transforms.Get(index).value;
                ref readonly var position = ref _aspect.Position.Get(index).value;
                
                transform.position = position;
            }
        }
    }
}