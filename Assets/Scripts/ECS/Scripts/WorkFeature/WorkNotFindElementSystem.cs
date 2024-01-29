using ECS.Scripts.Boot;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace ECS.Scripts.WorkFeature
{
    public sealed class WorkNotFindElementSystem : IProtoRunSystem
    {
        [DI] private readonly MainAspect _aspect;
        public void Run()
        {
            foreach (var entityProcess in _aspect.WorkProcessIt)
            {
                if (!_aspect.TargetWork.Has(entityProcess))
                {
                    _aspect.Owners.Get(entityProcess).value.Unpack(_aspect.World(), out var entityOwner);
                    
                    _aspect.CurrentWork.Del(entityOwner);
                    
                    _aspect.World().DelEntity(entityProcess);
                }
            }
        }
    }
}