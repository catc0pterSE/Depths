using Leopotam.EcsProto;

namespace ECS.Scripts.WorkFeature
{
    public interface INewWork
    {
        public bool IsDone();
        public void Apply(ProtoEntity entity);
    }
}