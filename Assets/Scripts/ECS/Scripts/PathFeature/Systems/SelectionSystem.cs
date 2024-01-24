using ECS.Scripts.Boot;
using Grid;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using Level;
using UnityEngine;

namespace ECS.Scripts.PathFeature.Systems
{
    public sealed class SelectionSystem : IProtoRunSystem
    {
        private bool _startSelected;
        
        private Vector3 _startPos;
        private Vector3 _endPos;

        private IGrid _grid;

        [DI] private readonly MainAspect _mainAspect;
        [DI] private readonly SelectionAspect _selectionAspect;
        [DI] private readonly SelectionView _selectionView;
        public void Run()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startPos = Input.mousePosition;
                
                _selectionView.gameObject.SetActive(true);
                
                foreach (var protoEntity in _selectionAspect.SelectedIt)
                {
                    _selectionAspect.Selected.Del(protoEntity);
                }
            }

            if (Input.GetMouseButton(0))
            {
                _endPos = Input.mousePosition;
                
                _selectionView.SetScreen(_startPos, _endPos);
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                
                _selectionView.gameObject.SetActive(false);
                _selectionView.SetScreen(Vector2.zero, Vector2.zero);
                
                var startPos = new Vector3Int(Mathf.RoundToInt(_startPos.x), Mathf.RoundToInt(_startPos.y));
                var endPos = new Vector3Int(Mathf.RoundToInt(_endPos.x), Mathf.RoundToInt(_endPos.y));
                
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
                
                if (_startPos.x < _endPos.x)
                {
                    startY = startPos.y;
                    endY = endPos.y;
                }
                else
                {
                    startY =  endPos.y;
                    endY = startPos.y;
                }
                
                for (int x = startX ; x < endX; x++)
                {
                    for (int y = startY; y < endY; y++)
                    {
                        var cell = _grid.GetCell(new Vector2Int(x,y));

                        if (cell.HasEntityWithComponent(_selectionAspect.CanSelect, _mainAspect, out var entity))
                        {
                            _selectionAspect.SelectedEvent.Add(entity);
                        }
                    }
                }
                
                _startSelected = false;
            }
        }
    }
}