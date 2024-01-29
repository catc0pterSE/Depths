using ECS.Scripts.Boot;
using Leopotam.EcsProto;
using Leopotam.EcsProto.Ai.Utility;
using Leopotam.EcsProto.QoL;

namespace ECS.Scripts.WorkFeature
{
    public sealed class UtilityCreateRequestSystem : IProtoRunSystem {
        [DI] readonly MainAspect _mainAspect = default;
        [DI] readonly AiUtilityModuleAspect _aiUtility = default;

        public void Run () 
        {
            foreach (var entity in _mainAspect.AISolutionIt)
            {
                _aiUtility.Request (entity);
            }
        }
    }
}