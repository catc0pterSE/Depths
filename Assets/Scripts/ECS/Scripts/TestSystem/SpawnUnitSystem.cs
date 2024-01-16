using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Boot
{
    public sealed class SpawnUnitSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
		
        private readonly StaticData _staticData;
        public void Run()
        {
            if (!Input.GetKeyDown(KeyCode.Space))
            {
                return;
            }
            
            var instanceObject = Object.Instantiate(_staticData.UnitPrefab);
            
            var entityUnit = _world.NewEntity();

            entityUnit.Get<Unit>();
            entityUnit.Get<TransformRef>().value = instanceObject.transform;
            entityUnit.Get<Selected>();
            entityUnit.Get<Position>().value = new Vector3(0, 1f, 0);
        }
    }
}