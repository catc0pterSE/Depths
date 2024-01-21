using System;
using Leopotam.Ecs;
using Leopotam.EcsProto.QoL;

namespace ECS.Scripts.Boot
{
    public interface ISelectionService
    {
        public event Action<ProtoPackedEntity, MainAspect> OnUnitSelected;
        public void SelectUnit(ProtoPackedEntity entity, MainAspect aspect);
    }
}