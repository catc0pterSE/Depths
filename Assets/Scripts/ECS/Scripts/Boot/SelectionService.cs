using System;
using Leopotam.Ecs;

namespace ECS.Boot
{
    public sealed class SelectionService : ISelectionService
    {
        public event Action<EcsEntity> OnUnitSelected;
        public void SelectUnit(EcsEntity entity)
        {
            OnUnitSelected?.Invoke(entity);
        }
    }
}