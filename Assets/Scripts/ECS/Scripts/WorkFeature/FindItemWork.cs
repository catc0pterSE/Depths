using ECS.Scripts.Boot;
using ECS.Scripts.GeneralComponents;
using ECS.Scripts.Path.Component;
using ECS.Scripts.TestSystem;
using Leopotam.Ecs;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Scripts.WorkFeature
{
    public class FindMineWork : IWork
    {
        private readonly EcsFilter<MiningTag, Position>.Exclude<ItemBusy> _items;
        private readonly MainAspect _aspect;
        public bool IsDone()
        {
            return _aspect.MiningFree.Len() != 0;
        }
        public void GiveWork(ProtoEntity entity)
        {
        }
    }
    public class FindItemWork : IWork
    {
        private readonly MainAspect _aspect;
        
        public FindItemWork(MainAspect aspect)
        {
            _aspect = aspect;
        }
        public bool IsDone()
        {
            return _aspect.ItemsFree.Len() != 0;
        }
        public void GiveWork(ProtoEntity entity)
        {
        }
    }
}