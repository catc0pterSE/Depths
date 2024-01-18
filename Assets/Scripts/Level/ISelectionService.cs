using System;
using Leopotam.Ecs;

namespace ECS.Scripts.Boot
{
    public interface ISelectionService
    {
        public event Action<EcsEntity> OnUnitSelected;
        public void SelectUnit(EcsEntity entity);
    }
}