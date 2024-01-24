using System;
using ECS.Scripts.Boot;
using Leopotam.EcsProto.QoL;

namespace Level
{
    public interface ISelectionService
    {
        public event Action<ProtoPackedEntity, MainAspect> OnUnitSelected;
        public void SelectUnit(ProtoPackedEntity entity, MainAspect aspect);
    }
}