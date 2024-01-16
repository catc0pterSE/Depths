using Leopotam.Ecs;

namespace ECS.Boot
{
    public sealed class PathMoveSystem : IEcsRunSystem
    {
        private readonly SceneData _sceneData;
        private readonly RuntimeData _runtimeData;
        private readonly StaticData _staticData;

        private readonly EcsFilter<Unit, Position, Path> _unitsPath;

        public void Run()
        {
            foreach (var index in _unitsPath)
            {
                ref var position = ref _unitsPath.Get2(index).value;
                ref var points = ref _unitsPath.Get3(index);
                
                var dir = points.value[points.index].Position - position;
                dir.y = 0;
                
                if(dir.sqrMagnitude < 0.01f)
                {
                    points.index++;

                    if (points.index >= points.value.Count)
                    {
                        var entity = _unitsPath.GetEntity(index);
                        entity.Del<Path>();
                    }
                }
                
                position += dir.normalized * (_runtimeData.deltaTime * 10f);
            }
        }
    }
}