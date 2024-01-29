using DefaultNamespace;
using ECS.Scripts.Boot;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Level;

namespace ECS.Scripts.WorkFeature
{
    public sealed class AddCellSystem : IProtoRunSystem
    {
        [DI] private readonly MainAspect _mainAspect;
        [DI] private readonly PathFindingService _path;
        public void Run()
        {
            foreach (var protoEntity in _mainAspect.AddCellIt)
            {
                var grid = _path.Grid;

                ref readonly var position = ref _mainAspect.Position.Get(protoEntity).value;
                
                var cell = grid.GetCell(position.FloorPositionInt2());

                var pack = _mainAspect.World().PackEntityWithWorld(protoEntity);
                
                cell.AddEntity(pack);
                
                _mainAspect.AddCell.Del(protoEntity);

            }
        }
    }
}