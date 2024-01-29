using ECS.Scripts.Boot;
using ECS.Scripts.TestSystem;
using Leopotam.EcsProto;
using Leopotam.EcsProto.Ai.Utility;
using Leopotam.EcsProto.QoL;

namespace ECS.Scripts.WorkFeature
{
    public sealed class SearchFoodSolver : IAiUtilitySolver 
    {
        [DI] private readonly MainAspect _mainAspect = default;
        public float Solve(ProtoEntity entity)
        {
            ref var stats = ref _mainAspect.StatAspect.Stats.Get(entity).value;
            
            stats[StatType.Hungry].Unpack(_mainAspect.World() ,out var statEntity);
            
            var stat = _mainAspect.StatAspect.Stat.Get(statEntity).value;
            
            if (stat < 70f) 
            {
                return 2f;
            }
            
            return 0f;
        }
        public void Apply(ProtoEntity entity)
        {
            _mainAspect.AddSolution(_mainAspect.FindFood, entity, out var aiSolution);
        }
    }
}