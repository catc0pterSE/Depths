using ECS.Scripts.Data;
using ECS.Scripts.GeneralComponents;
using ECS.Scripts.Path.Component;
using ECS.Scripts.WorkFeature;
using Leopotam.Ecs;
using Level;
using UnityEngine;

namespace ECS.Scripts.Path.Systems
{
    public sealed class CreateRandPathSystem : IEcsRunSystem
    {
        private readonly EcsFilter<Position, RandMove>.Exclude<Component.Path, WorkProcess> _units;
        private RuntimeData _runtimeData;

        private LevelPN _levelPn;
        public void Run()
        {
            foreach (var index in _units)
            {
                var entity = _units.GetEntity(index);
                ref var position = ref _units.Get1(index).value;
                ref var randMove = ref _units.Get2(index);

                randMove.time -= _runtimeData.deltaTime;
                
                if (randMove.time <= 0)
                {
                    randMove.time = Random.Range(1f, 2f);
                    var randDirection = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
                    
                    
                    
                    if (_levelPn.OutBounds(position + randDirection))
                    {
                        continue;
                    }
       
                    entity.Get<TargetPath>().value = position + randDirection;
                }
            }
        }
    }
}