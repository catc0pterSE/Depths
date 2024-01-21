using Leopotam.Ecs;
using Leopotam.EcsProto;

namespace ECS.Scripts.WorkFeature
{
    public interface IWork
    {
        public bool IsDone();
        public void GiveWork(ProtoEntity entity);
    }
}