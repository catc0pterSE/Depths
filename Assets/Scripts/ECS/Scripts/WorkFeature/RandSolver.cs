using ECS.Scripts.Boot;
using Leopotam.EcsProto;
using Leopotam.EcsProto.Ai.Utility;
using Leopotam.EcsProto.QoL;

namespace ECS.Scripts.WorkFeature
{
    public sealed class RandSolver : IAiUtilitySolver 
    {
        [DI] private readonly MainAspect _mainAspect = default;
        public float Solve(ProtoEntity entity)
        {
            return 0.1f;
        }
        public void Apply(ProtoEntity entity)
        {
            _mainAspect.AddSolution(_mainAspect.RandMove, entity, out _);
        }
    }
}