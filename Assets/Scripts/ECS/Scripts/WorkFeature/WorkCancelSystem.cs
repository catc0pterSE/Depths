using ECS.Scripts.Boot;
using ECS.Scripts.Data;
using Leopotam.Ecs;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace ECS.Scripts.WorkFeature
{
    public sealed class WorkCancelSystem : IProtoRunSystem
    {
        [DI] private readonly SceneData _sceneData;
        [DI] private readonly RuntimeData _runtimeData;
        [DI] private readonly StaticData _staticData;
        
        
        [DI] private readonly MainAspect _aspect;


        public void Run()
        {
            foreach (var index in _aspect.CancelWorkF)
            {
                _aspect.WorkProcess.Del(index);
                _aspect.CancelWork.Del(index);
            }
        }

        
    }
}