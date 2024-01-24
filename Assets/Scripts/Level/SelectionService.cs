using System;
using ECS.Scripts.Boot;
using Leopotam.EcsProto.QoL;

namespace Level
{
    public sealed class SelectionService : ISelectionService
    {
        public event Action<ProtoPackedEntity, MainAspect> OnUnitSelected;
        public void SelectUnit(ProtoPackedEntity entity, MainAspect aspect)
        {
            OnUnitSelected?.Invoke(entity, aspect);
        }
    }
}