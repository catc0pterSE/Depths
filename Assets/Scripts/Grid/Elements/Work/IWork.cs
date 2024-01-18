using Leopotam.Ecs;

namespace ECS.Scripts.Work
{
    public interface IWork
    {
        public bool IsDone();
        public void GiveWork(EcsEntity entity);
    }
}