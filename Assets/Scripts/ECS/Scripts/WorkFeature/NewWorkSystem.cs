using ECS.Scripts.Boot;
using ECS.Scripts.Data;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;

namespace ECS.Scripts.WorkFeature
{
    public sealed class NewWorkSystem : IProtoRunSystem
    {
        [DI] private readonly SceneData _sceneData;
        [DI] private readonly RuntimeData _runtimeData;
        [DI] private readonly StaticData _staticData;

        [DI] private readonly MainAspect _aspect;
        public void Run()
        {
            foreach (var work in _runtimeData.Works)
            {
                if(!work.IsDone()) continue;
                
                foreach (var unitEntity in _aspect.WorkersNotWorking)
                {
                    
                    var works = _aspect.Works.Get(unitEntity).value;
                    
                    RecalculationPriority(works, work, unitEntity);
                }
            }
            
            foreach (var unitEntity in _aspect.NewWorkIt)
            {
                ref var unitNewWork = ref _aspect.NewWork.Get(unitEntity);
                
                _aspect.CurrentWork.Add(unitEntity).value = unitNewWork.newWork;
                        
                _aspect.NewWork.Del(unitEntity);
                _aspect.WorkCost.Del(unitEntity);
            }
        }

        private void RecalculationPriority(Work[] works, INewWork work, ProtoEntity unitEntity)
        {
            foreach (var unitWork in works)
            {
                if (unitWork.valueNew == work)
                {
                    if(unitWork.Order == 0) continue;
                            
                    ref var workCost = ref _aspect.WorkCost.GetOrAdd(unitEntity, out _).value;

                    if (unitWork.Order > workCost)
                    {
                        workCost = unitWork.Order;
                        
                        ref var newWork = ref _aspect.NewWork.GetOrAdd(unitEntity, out _);
                                
                        newWork.newWork = work;
                    }
                            
                    break;
                }
            }
        }
    }
}