using Leopotam.Ecs;

namespace ECS.Scripts.WorkFeature
{
    public interface IWork
    {
        public bool IsDone();
        public void GiveWork(EcsEntity entity);
    }
}