using ECS.Scripts.Data;
using Leopotam.Ecs;

namespace ECS.Scripts.WorkFeature
{
    public sealed class WorkCancelSystem : IEcsRunSystem
    {
        private readonly SceneData _sceneData;
        private readonly RuntimeData _runtimeData;
        private readonly StaticData _staticData;

        private readonly EcsFilter<WorkProcess, CancelWork> _filter;

        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var entity = ref _filter.GetEntity(index);
                entity.Del<WorkProcess>();
                entity.Del<CancelWork>();
            }
        }

        
    }
}