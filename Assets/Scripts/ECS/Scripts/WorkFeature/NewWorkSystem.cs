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

        [DI] private readonly MainAspect _mainAspect;
        public void Run()
        {
            foreach (var work in _runtimeData.Works)
            {
                if(!work.IsDone()) continue;

                int maxOrder = 0;
                ProtoEntity worker = default;
                
                foreach (var unitEntity in _mainAspect.WorkersNotWorking)
                {
                    var works = _mainAspect.Works.Get(unitEntity).value;
                    
                    var order = GetOrder(works, work);

                    if (order > maxOrder)
                    {
                        maxOrder = order;
                        worker = unitEntity;
                    }
                    //RecalculationPriority(works, work, unitEntity);
                }
                
                ref var newWork = ref _mainAspect.NewWork.Add(worker);
                newWork.newWork = work;
            }
            
            foreach (var unitEntity in _mainAspect.NewWorkIt)
            {
                ref var unitNewWork = ref _mainAspect.NewWork.Get(unitEntity);
                
                _mainAspect.CurrentWork.Add(unitEntity).value = unitNewWork.newWork;
                        
                _mainAspect.NewWork.Del(unitEntity);
                _mainAspect.WorkCost.Del(unitEntity);
            }
        }

        private int GetOrder(Work[] works, INewWork work)
        {
            foreach (var unitWork in works)
            {
                if (unitWork.valueNew == work)
                {
                    return unitWork.Order;
                }
            }
            
            return 0;
        }
        
        private void RecalculationPriority(Work[] works, INewWork work, ProtoEntity unitEntity)
        {
            foreach (var unitWork in works)
            {
                if (unitWork.valueNew == work)
                {
                    if(unitWork.Order == 0) continue;
                            
                    ref var workCost = ref _mainAspect.WorkCost.GetOrAdd(unitEntity, out _).value;

                    if (unitWork.Order > workCost)
                    {
                        workCost = unitWork.Order;
                        
                        ref var newWork = ref _mainAspect.NewWork.GetOrAdd(unitEntity, out _);
                                
                        newWork.newWork = work;
                    }
                            
                    break;
                }
            }
        }
    }
}