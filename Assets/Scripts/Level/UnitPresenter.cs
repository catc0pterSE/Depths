using ECS.Scripts.TestSystem;
using Leopotam.Ecs;

namespace ECS.Scripts.Boot
{
    public sealed class UnitPresenter
    {
        private EcsEntity _entity;
        private readonly UnitWindow _unitWindow;
        private readonly ISelectionService _selectionService;
        public UnitPresenter(ISelectionService selectionService, UnitWindow unitWindow)
        {
            _selectionService = selectionService;
            _selectionService.OnUnitSelected += OnUnitSelected;
			
            _unitWindow = unitWindow;
        }

        public void OnUnitSelected(EcsEntity entity)
        {
            _entity = entity;

            var stats = _entity.Get<Stats>().value;

            string description = "";
			
            foreach (var statEntity in stats.Values)
            {
                ref readonly var stat = ref statEntity.Get<Stat>();
                description += stat.type + ": " + stat.TotalValue() + "\n";
            } 

            _unitWindow.SetStats(description);
			
            var parts = _entity.Get<Body>().parts;
			
            description = "";
			
            foreach (var partEntity in parts.Values)
            {
                ref readonly var part = ref partEntity.Get<Part>().value;
                ref readonly var health = ref partEntity.Get<Health>().value;
                description += part + ": " + health + "\n";
            } 
			
            _unitWindow.SetParts(description);
			
            _unitWindow.Show();
        }
    }
}