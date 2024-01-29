using DefaultNamespace;
using ECS.Scripts.Boot;
using ECS.Scripts.Data;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Level;
using UnityEngine;

namespace ECS.Scripts.WorkFeature
{
    public sealed class MineDiedSystem : IProtoRunSystem
    {
        [DI]  private readonly StaticData _staticData;
        [DI] private readonly MainAspect _aspect;
        [DI] private readonly PathFindingService _path;
        
        public void Run()
        {
            foreach (var entityMine in _aspect.MiningDied)
            {
                ref var health = ref _aspect.Health.Get(entityMine).value;
                
                if(health <= 0)
                {
                    _aspect.MiningTag.Del(entityMine);
                    _aspect.Transforms.Get(entityMine).value.gameObject.SetActive(false);
                    
                    Debug.Log("mining die");
                    
                    ref readonly var position = ref _aspect.Position.Get(entityMine).value;
                    
                    var instanceObject = Object.Instantiate(_staticData.ItemPrefab);
            
                    var entityUnit = _aspect.World().NewEntity();
                    
                    _aspect.SelectionAspect.CanSelect.Add(entityUnit);
                    _aspect.Items.Add(entityUnit);
                    _aspect.Position.Add(entityUnit).value = position;
                    _aspect.Transforms.Add(entityUnit).value = instanceObject.transform;

                    var packedEntity = _aspect.World().PackEntityWithWorld(entityUnit);

                    var floor = position.FloorPositionInt2();
                    
                    _path.Grid.Map[floor.x, floor.y].AddEntity(packedEntity);

                }
            }
        }

        
    }
}