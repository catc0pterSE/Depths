using DefaultNamespace;
using ECS.Scripts.Boot;
using ECS.Scripts.Data;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Level;
using UnityEngine;

namespace ECS.Scripts.TestSystem
{
    public sealed class SpawnItemSystem : IProtoRunSystem
    {
        [DI]private readonly StaticData _staticData;
        [DI] private readonly MainAspect _aspect;
        [DI] private readonly PathFindingService _path;

        public void Run()
        {
            if (!Input.GetKeyDown(KeyCode.E))
            {
                return;
            }
            int count = 100;

            while (count > 0)
            {
                var instanceObject = Object.Instantiate(_staticData.MinePrefab);

                var entityUnit = _aspect.World().NewEntity();
                
                _aspect.MiningTag.Add(entityUnit);
                _aspect.SelectionAspect.CanSelect.Add(entityUnit);
                _aspect.Health.Add(entityUnit).value = 5f;
                _aspect.Transforms.Add(entityUnit).value = instanceObject.transform;
                
                var pos =  new Vector3(Random.Range(0f, 100f), Random.Range(0f, 100f)).FloorPositionInt2();
                _aspect.Position.Add(entityUnit).value = new Vector3(pos.x, pos.y);

                var packedEntity = _aspect.World().PackEntityWithWorld(entityUnit);
                
                _path.Grid.Map[pos.x, pos.y].AddEntity(packedEntity);
                
                count--;
            }
        }

        
    }
}