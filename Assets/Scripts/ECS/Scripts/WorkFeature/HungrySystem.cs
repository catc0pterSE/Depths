using ECS.Scripts.Boot;
using ECS.Scripts.Data;
using ECS.Scripts.TestSystem;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace ECS.Scripts.WorkFeature
{
    public sealed class HungrySystem : IProtoRunSystem
    {
        [DI] private readonly MainAspect _mainAspect = default;
        [DI] private readonly RuntimeData _runtimeData = default;
        public void Run()
        {
            foreach (var protoEntity in _mainAspect.StatAspect.StatsIt)
            {
                var stats = _mainAspect.StatAspect.Stats.Get(protoEntity).value;
               
                stats[StatType.Hungry].Unpack(_mainAspect.World() ,out var statEntity);
                
                ref var amountHungry = ref _mainAspect.StatAspect.Stat.Get(statEntity).value;

                amountHungry -= _runtimeData.deltaTime;
            }
        }
    }
}