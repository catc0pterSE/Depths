using ECS.Scripts.Boot;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace ECS.Scripts.WorkFeature
{
    public sealed class NewFindItemWork : INewWork
    {
        private MainAspect _mainAspect;
        public NewFindItemWork(MainAspect aspect) => _mainAspect = aspect;
        public bool IsDone()
        {
            return _mainAspect.ItemsFree.Len() > 0;
        }
        public void Apply(ProtoEntity entity)
        {
            _mainAspect.FindItemProcess.Add(entity);
            _mainAspect.FindNearElement.Add(entity).Iterator = _mainAspect.ItemsFree;
        }
    }
}