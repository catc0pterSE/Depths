using System;
using System.Collections.Generic;
using ECS.Scripts.Boot;
using ECS.Scripts.CharacterComponent;
using ECS.Scripts.Data;
using ECS.Scripts.GeneralComponents;
using ECS.Scripts.Path.Component;
using ECS.Scripts.ProviderComponents;
using ECS.Scripts.TestSystem;
using Leopotam.Ecs;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ECS.Scripts.WorkFeature
{

    public sealed class MineDiedSystem : IProtoRunSystem
    {
        [DI]  private readonly StaticData _staticData;
        [DI] private readonly MainAspect _aspect;

        private readonly EcsFilter<MiningTag, Health, Position> _filter;

        public void Run()
        {
            foreach (var index in _aspect.MiningDied)
            {
                ref var health = ref _aspect.Health.Get(index).value;
                
                if(health <= 0)
                {
                    _aspect.MiningTag.Del(index);
                    _aspect.Transforms.Get(index).value.gameObject.SetActive(false);
                    
                    
                    ref readonly var position = ref _aspect.Position.Get(index).value;
                    
                    var instanceObject = Object.Instantiate(_staticData.ItemPrefab);
            
                    var entityUnit = _aspect.World().NewEntity();
                    
                    _aspect.Items.Add(entityUnit);
                    _aspect.Position.Add(entityUnit).value = position;
                    _aspect.Transforms.Add(entityUnit).value = instanceObject.transform;
                }
            }
        }

        
    }
    
    public sealed class WorkSystem : IProtoInitSystem, IProtoRunSystem
    {
        [DI] private readonly SceneData _sceneData;
        [DI] private readonly RuntimeData _runtimeData;
        [DI] private readonly StaticData _staticData;
        
        private readonly EcsFilter<MiningTag, Position>.Exclude<ItemBusy> _mining;
           
        [DI] private readonly MainAspect _aspect;
        
        private Dictionary<WorkType, Func<bool>> _conditions;
        
        
                      
        [BurstCompile]
        private struct DistanceJob : IJob
        {
            public NativeArray<WorkerData> Workers;
            public NativeArray<DistanceData> Items;
            
            public float TotalDistance;
            [BurstCompile]
            public void Execute()
            {
                for (int indexWorker = 0; indexWorker < Workers.Length; indexWorker++)
                {
                    if (indexWorker >= Items.Length - 1)
                    {
                        break;
                    }
                    
                    var worker = Workers[indexWorker];;
                    TotalDistance = 100000;
                        
                    for (int indexItem = 0; indexItem < Items.Length; indexItem++)
                    {
                        var item = Items[indexItem];
                        
                        if(item.Exit) continue;
                        
                        var distanceTwo =  math.distancesq(item.PositionItem, worker.Position);
                    
                        if (TotalDistance > distanceTwo)
                        {
                            TotalDistance = distanceTwo;

                            if (worker.ItemIndex > -1)
                            {
                                var oldItem = Items[worker.ItemIndex];
                                oldItem.Exit = false;
                                Items[worker.ItemIndex] = oldItem;
                            }

                            worker.ItemIndex = item.ItemIndex;
                            
                            worker.PositionItem = item.PositionItem;

                            item.Exit = true;
                                
                            Items[indexItem] = item;
                            Workers[indexWorker] = worker;
                        }
                    }
                }
            }
        }
        
        public struct WorkerData
        {
            [ReadOnly]  public float3 Position;
            [ReadOnly]  public int WorkerIndex;
            
            [WriteOnly] public float3 PositionItem;
            [WriteOnly] public int ItemIndex;
        }
        public struct DistanceData
        {
            [ReadOnly] public float3 PositionItem;
            [ReadOnly] public int ItemIndex;

            [WriteOnly] public bool Exit;
        }
        
        
        
        
        public enum WorkType
        {
            FindItem,
            Mining,
        }
        public void Run()
        {
            foreach (var worker in _aspect.WorkersNotWorking)
            {
                var work = _aspect.Works.Get(worker);
                foreach (var work1 in work.value)
                {
                    if (work1.value.IsDone())
                    {
    
                        work1.value.GiveWork(worker);

                        _aspect.WorkProcess.Add(worker);
                    }
                }
            }


            // if (!_mining.IsEmpty())
            // {
            //
            //     if(_workers.GetEntitiesCount() == 0) return;
            //     
            //     var distanceWorkers = new NativeArray<WorkerData>(_workers.GetEntitiesCount(), Allocator.TempJob);
            //     var distanceItems = new NativeArray<DistanceData>(_mining.GetEntitiesCount(), Allocator.TempJob);
            //     
            //     for (int i = 0; i < distanceWorkers.Length; i++)
            //     {
            //
            //         _workers.GetEntity(i).Get<TargetPath>().value = Vector3.zero;
            //
            //         var workerData = new WorkerData();
            //         
            //         workerData.PositionItem = _workers.Get2(i).value;
            //         
            //         workerData.WorkerIndex = i;
            //         
            //         workerData.ItemIndex = -1;
            //         
            //         distanceWorkers[i] = workerData;
            //     }
            //     
            //     for (int i = 0; i < distanceItems.Length; i++)
            //     {
            //         var distanceData = new DistanceData();
            //         
            //         distanceData.PositionItem = _mining.Get2(i).value;
            //         
            //         distanceData.ItemIndex = i;
            //         
            //         distanceData.Exit = false;
            //         
            //         distanceItems[i] = distanceData;
            //     }
            //     
            //     DistanceJob moveJob = new DistanceJob { Workers = distanceWorkers, Items = distanceItems, TotalDistance = 100000f};
            //     
            //     var jobHandle = moveJob.Schedule();
            //     jobHandle.Complete();
            //     
            //     for (int i = 0; i < distanceWorkers.Length; i++)
            //     {
            //         var worker = distanceWorkers[i];
            //         ref var entity = ref _workers.GetEntity(worker.WorkerIndex);
            //         if (worker.ItemIndex > -1)
            //         {
            //             
            //             ref var findEntity = ref _mining.GetEntity(worker.ItemIndex);
            //             
            //             findEntity.Get<ItemPlaced>();
            //             
            //             entity.Get<WorkProcess>();
            //             entity.Get<MineProcess>().ItemEntity = findEntity;
            //             entity.Get<TargetPath>().value = worker.PositionItem;
            //         }
            //     }
            //     
            //     distanceItems.Dispose();
            //     distanceWorkers.Dispose();
            // }
        }

        public void Init(IProtoSystems systems)
        {
            _conditions = new Dictionary<WorkType, Func<bool>>()
            {
                [WorkType.Mining] = () => !_mining.IsEmpty(),
            };
        }
    }
}