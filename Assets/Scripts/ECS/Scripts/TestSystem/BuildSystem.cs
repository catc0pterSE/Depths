using ECS.Scripts.Boot;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Level;
using UnityEngine;

namespace ECS.Scripts.TestSystem
{
    public sealed class BuildSystem : IProtoRunSystem
    {
        [DI] private readonly MainAspect _mainAspect;
        [DI] private readonly PathFindingService _pathFindingService;

        public void Run()
        {
            foreach (var entity in _mainAspect.BuildFilter)
            {
                var buildWall = _mainAspect.Build.Get(entity);
                var mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                buildWall.transform.position = mouseWorldPosition;

                if (Input.GetMouseButtonUp(0))
                {
                    var grid = _pathFindingService.Grid;

                    if (grid.OutBounds(mouseWorldPosition))
                        continue;

                    var cell = grid.FromWorldToCell(mouseWorldPosition);

                    if (cell.HasEntity())
                        continue;

                    var packedEntity = _mainAspect.World().PackEntity(entity);
                    cell.AddEntity(packedEntity);
                    
                    _mainAspect.Build.Del(entity);
                }
            }
        }
    }
}