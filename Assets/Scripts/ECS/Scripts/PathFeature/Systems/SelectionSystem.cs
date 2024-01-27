using System.Collections.Generic;
using ECS.Scripts.Boot;
using ECS.Scripts.Data;
using ECS.Scripts.GeneralComponents;
using Grid;
using Grid.Elements.Work.Cell;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Level;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ECS.Scripts.PathFeature.Systems
{
    public interface ISpatialHash
    {
        public void UpdatePosition(ProtoPackedEntity entity, Vector3 position);
    }
    public sealed class SpatialHash : ISpatialHash
    {
        private IGrid _grid;

        private Dictionary<ProtoPackedEntity, CellPFModel> _cells = new Dictionary<ProtoPackedEntity, CellPFModel>(100);

        public SpatialHash(IGrid grid)
        {
            _grid = grid;
        }

        public void UpdatePosition(ProtoPackedEntity entity, Vector3 position)
        {
            var cell = _grid.FromWorldToCell(position);
            if (!cell.HasEntity(entity))
            {
                cell.AddEntity(entity);
            }
            
            if(_cells.TryGetValue(entity, out var oldCell))
            {
                if(cell.GridPosition == oldCell.GridPosition) return;
                
                oldCell.RemoveEntity(entity);
                _cells.Remove(entity);
            }
            else
            {
                _cells.Add(entity, cell);
            }
        }
    }
    public sealed class SelectionSystem : IProtoRunSystem
    {
        private bool _startSelected;
        
        private Vector3 _startPos;
        private Vector3 _endPos;

        [DI] private PathFindingService _grid;

        [DI] private readonly MainAspect _mainAspect;
        [DI] private readonly SelectionAspect _selectionAspect;
        [DI] private readonly SelectionView _selectionView;
        [DI] private StaticData _staticData;

        public void Run()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                _startPos = Input.mousePosition;
                
                _selectionView.gameObject.SetActive(true);
                
                foreach (var protoEntity in _selectionAspect.SelectedIt)
                {
                    _selectionAspect.Selected.Del(protoEntity);
                }
                
                foreach (var protoEntity in _selectionAspect.SelectedViewIt)
                {
                    var tr = _selectionAspect.SelectedView.Get(protoEntity).value;
                    Object.Destroy(tr.gameObject);
                    _selectionAspect.SelectedView.Del(protoEntity);
                }
                
                _startSelected = true;
            }

            if (Input.GetMouseButton(0) && _startSelected)
            {
                _endPos = Input.mousePosition;
                
                _selectionView.SetScreen(_startPos, _endPos);
            }
            
            if (Input.GetMouseButtonUp(0) && _startSelected)
            {
                
                _selectionView.gameObject.SetActive(false);
                _selectionView.SetScreen(Vector2.zero, Vector2.zero);

                var camera = Camera.main;
                
                var startPos = ConvertSpaceToGridPosition(camera, out var endPos);
                
                
                int startX;
                int endX;
                
                if (_startPos.x < _endPos.x)
                {
                    startX = startPos.x;
                    endX = endPos.x;
                }
                else
                {
                    startX =  endPos.x;
                    endX = startPos.x;
                }
                
                int startY;
                int endY;
                
                if (_startPos.y < _endPos.y)
                {
                    startY = startPos.y;
                    endY = endPos.y;
                }
                else
                {
                    startY =  endPos.y;
                    endY = startPos.y;
                }

                var world = _mainAspect.World();
                for (int x = startX ; x < endX; x++)
                {
                    for (int y = startY; y < endY; y++)
                    {
                        var cell = _grid.Grid.GetCell(new Vector2Int(x,y));
                 
                        foreach (var protoPackedEntity in cell.GetEntities())
                        {
                            protoPackedEntity.Unpack(world, out var entity);
                            
                            if (_selectionAspect.CanSelect.Has(entity))
                            {
                                _selectionAspect.SelectedEvent.GetOrAdd(entity, out _);
                            }
                        }
                    }
                }
                
                _startSelected = false;
            }
        }

        private Vector3Int ConvertSpaceToGridPosition(Camera camera, out Vector3Int endPos)
        {
            var worldStart = camera.ScreenToWorldPoint(_startPos);
            var worldEnd = camera.ScreenToWorldPoint(_endPos);
            
            worldStart = _grid.Grid.ClampPosition(worldStart);
            worldEnd = _grid.Grid.ClampPosition(worldEnd);
            
            
            var startPos = new Vector3Int(Mathf.FloorToInt(worldStart.x), Mathf.FloorToInt(worldStart.y));
            endPos = new Vector3Int(Mathf.FloorToInt(worldEnd.x), Mathf.FloorToInt(worldEnd.y));
            return startPos;
        }
    }
}