using ECS.Boot;
using Leopotam.Ecs;

namespace ECS.Scripts.Work
{
    
    
    public sealed class WorkSystem : IEcsRunSystem
    {
        private readonly SceneData _sceneData;
        private readonly RuntimeData _runtimeData;
        private readonly StaticData _staticData;

        private readonly EcsFilter<Works>.Exclude<WorkProcess> _filter;

        public void Run()
        {
            foreach (var index in _filter)
            {
                ref var works = ref _filter.Get1(index);
                foreach (var work in works.value)
                {
                    if (work.value.IsDone())
                    {
                        ref var entity = ref _filter.GetEntity(index);
                        
                        work.value.GiveWork(entity);

                        entity.Get<WorkProcess>();
                        
                        break;
                    }
                }
            }
        }
    }
}