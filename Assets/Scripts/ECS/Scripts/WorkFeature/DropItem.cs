using DefaultNamespace;
using ECS.Scripts.Boot;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Level;

namespace ECS.Scripts.WorkFeature
{
    public sealed class DropItem : IProtoRunSystem
    {
        [DI] private readonly MainAspect _mainAspect;
        [DI] private readonly PathFindingService _path;
        public void Run()
        {
            foreach (var entity in _mainAspect.DropItemIt)
            {
                ref var item = ref _mainAspect.ItemsInHand.Get(entity);;
                
                if(item.packedEntity.Unpack(_mainAspect.World(), out var itemEntity))
                {
                    _mainAspect.Transforms.Get(itemEntity).value.SetParent(null);
                    
                    _mainAspect.AddCell.Add(itemEntity);
                    
                    _mainAspect.Position.Get(itemEntity).value = _mainAspect.Position.Get(entity).value.FloorPosition();
                
                    _mainAspect.Sync.Del(itemEntity);
                    
                    _mainAspect.ItemsInHand.Del(entity);
                    
                    _mainAspect.Drop.Del(entity);
                }
            }
        }
    }
}