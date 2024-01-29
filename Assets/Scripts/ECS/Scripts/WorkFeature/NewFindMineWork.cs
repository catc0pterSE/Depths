using ECS.Scripts.Boot;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace ECS.Scripts.WorkFeature
{
    public sealed class NewFindMineWork : INewWork
    {
        private MainAspect _mainAspect;
        public NewFindMineWork(MainAspect aspect) => _mainAspect = aspect;
        public bool IsDone()
        {
            return _mainAspect.MiningFree.Len() > 0;
        }
        public void Apply(ProtoEntity entity)
        {
            _mainAspect.MineProcess.Add(entity);
            _mainAspect.FindNearElement.Add(entity).Iterator = _mainAspect.MiningFree;
        }
    }
}