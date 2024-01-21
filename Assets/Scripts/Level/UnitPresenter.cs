using ECS.Scripts.TestSystem;
using Leopotam.Ecs;
using Leopotam.EcsProto.QoL;

namespace ECS.Scripts.Boot
{
    public sealed class UnitPresenter
    {
        private ProtoPackedEntity _entity;
        private readonly UnitWindow _unitWindow;
        private readonly ISelectionService _selectionService;
        public UnitPresenter(ISelectionService selectionService, UnitWindow unitWindow)
        {
            _selectionService = selectionService;
            _selectionService.OnUnitSelected += OnUnitSelected;
			
            _unitWindow = unitWindow;
        }

        public void OnUnitSelected(ProtoPackedEntity entity, MainAspect aspect)
        {
            _entity = entity;
            _entity.Unpack(aspect.World(), out var unPackEntity);
            var stats = aspect.Stats.Get(unPackEntity).value;

            string description = "";
			
            foreach (var statEntity in stats.Values)
            {
                statEntity.Unpack(aspect.World(), out var unPackStatEntity);
                
                ref readonly var stat = ref aspect.Stat.Get(unPackStatEntity);
                
                description += stat.type + ": " + stat.TotalValue() + "\n";
            } 

            _unitWindow.SetStats(description);
			
            var parts = aspect.Bodies.Get(unPackEntity).parts;
			
            description = "";
			
            foreach (var partEntity in parts.Values)
            {
                partEntity.Unpack(aspect.World(), out var unPackPartEntity);
                
                ref readonly var part = ref aspect.Parts.Get(unPackPartEntity);
                ref readonly var health = ref aspect.Health.Get(unPackPartEntity);
                description += part + ": " + health + "\n";
            } 
			
            _unitWindow.SetParts(description);
			
            _unitWindow.Show();
        }
    }
}