using ECS.Scripts.Boot;
using Leopotam.EcsProto;
using Leopotam.EcsProto.Ai.Utility;
using Leopotam.EcsProto.QoL;
using UnityEngine;

namespace ECS.Scripts.WorkFeature
{
    public sealed class BeginCompleteWorkSolver : IAiUtilitySolver 
    {
        [DI] private readonly MainAspect _mainAspect = default;
        public float Solve(ProtoEntity entity)
        {
            if (_mainAspect.CurrentWork.Has(entity)) 
            {
                return 10f;
            }
            
            return 0f;
        }
        public void Apply(ProtoEntity entity)
        {
            if (_mainAspect.AddSolution(_mainAspect.WorkProcess, entity, out var solutionEntity))
            {
                Debug.Log("added solution");
                
                var position = _mainAspect.Position.Get(entity).value;
                
                _mainAspect.Position.Add(solutionEntity).value = position;
                
                _mainAspect.CurrentWork.Get(entity).value.Apply(solutionEntity);
            }
        }
    }
}