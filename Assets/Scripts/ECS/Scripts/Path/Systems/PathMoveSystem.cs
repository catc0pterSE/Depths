using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Boot
{
    public sealed class PathMoveSystem : IEcsRunSystem
    {
        private readonly SceneData _sceneData;
        private readonly RuntimeData _runtimeData;
        private readonly StaticData _staticData;

        private readonly EcsFilter<Position, Path> _unitsPath;
        
        private readonly EcsFilter<Position, Direction, Stats> _unitsMoveDirection;

        public void Run()
        {
            if (_unitsPath.IsEmpty()) return;
            
            foreach (var index in _unitsPath)
            {
                ref var position = ref _unitsPath.Get1(index).value;
                
                ref var points = ref _unitsPath.Get2(index);
                
                ref var entity = ref _unitsPath.GetEntity(index);
                
                var cellView = points.value[points.index];
                
                var dir = cellView.Position - position;
                dir.y = 0;
                
                if(dir.sqrMagnitude < 0.01f)
                {
                    points.index++;

                    if (points.index >= points.value.Count)
                    {
                        entity.Del<Direction>();
                        entity.Del<Path>();
                        
                        continue;
                    }
                }

                entity.Get<Direction>().value = dir.normalized;
            }

            foreach (var index in _unitsMoveDirection)
            {
                ref var position = ref _unitsMoveDirection.Get1(index).value;
                ref var direction = ref _unitsMoveDirection.Get2(index).value;
                var speed = _unitsMoveDirection.Get3(index).value[StatType.Speed].Get<Stat>().TotalValue();
                
                position += direction * (speed * _runtimeData.deltaTime);
                
            }
        }
        
       
    }
}