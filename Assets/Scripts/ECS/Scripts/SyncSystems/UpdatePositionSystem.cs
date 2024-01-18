using ECS.Scripts.Work;
using Leopotam.Ecs;

namespace ECS.Boot
{

    public sealed class UpdatePositionSystem : IEcsRunSystem
    {
        private readonly EcsFilter<TransformRef, Position>.Exclude<Sync> _syncPosition;
        
        public void Run()
        {
            foreach (var index in _syncPosition)
            {
                ref readonly var transform = ref _syncPosition.Get1(index);
                ref readonly var position = ref _syncPosition.Get2(index);
                
                transform.value.position = position.value;
            }
        }
    }
}