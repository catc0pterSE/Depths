using ECS.Scripts.Boot;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;
using UnityEngine.Pool;

namespace ECS.Scripts.PathFeature.Systems
{
    public sealed class CheckDistancePointInPathSystem : IProtoRunSystem
    {
        [DI] private MainAspect _aspect;
        [DI] private PathAspect _pathAspect;
        public void Run()
        {
            foreach (var index in _pathAspect.MovePath)
            {
                ref var position = ref _aspect.Position.Get(index).value;
                ref var points = ref _pathAspect.Path.Get(index);
                
                var positionPoint = points.value[points.index];
                
                var dir = positionPoint - position;
                
                if(dir.sqrMagnitude == 0f)
                {
                    points.index--;
                    if (points.index == -1)
                    {
                        ListPool<Vector3>.Release(points.value);
                        
                        _aspect.Direction.Del(index);
                        _pathAspect.Path.Del(index);
                        
                        continue;
                    }
                }
                
                _aspect.Direction.GetOrAdd(index, out var added).value = positionPoint;
            }
        }
    }
}