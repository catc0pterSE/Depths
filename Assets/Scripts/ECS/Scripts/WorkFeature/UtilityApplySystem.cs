using ECS.Scripts.Boot;
using Leopotam.EcsProto;
using Leopotam.EcsProto.Ai.Utility;
using Leopotam.EcsProto.QoL;

namespace ECS.Scripts.WorkFeature
{
    public sealed class UtilityApplySystem : IProtoRunSystem
    {
        [DI] readonly AiUtilityModuleAspect _aiUtility = default;
        [DI] readonly MainAspect _mainAspect = default;
        public void Run()
        {
            foreach (var entity in _mainAspect.AISolutionResponceIt)
            {
                ref var res = ref _aiUtility.ResponseEvent.Get(entity);
                res.Solver.Apply (entity);
            }
        }
    }
}