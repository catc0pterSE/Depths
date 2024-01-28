using System;
using ECS.Scripts.Boot;
using Leopotam.EcsProto.QoL;
using UnityEngine;
using UnityEngine.UI;

namespace ECS.Scripts.PathFeature.Systems
{
    public sealed class SelectionView : MonoBehaviour
    {
        [SerializeField] private RectTransform _box;
        [SerializeField] private SpriteRenderer _boxSprite;

        [SerializeField] private Button _miningButton;
        [SerializeField] private Button _itemButton;
        [SerializeField] private Button _zoneButton;

        private MainAspect _mainAspect;

        private SpriteRenderer _spriteRenderer;
        public void Construct(MainAspect aspect)
        {
            _mainAspect = aspect;
        }
        public void SetScreen(Vector2 startPos, Vector2 endPos)
        {
            var fromStartToEnd = endPos - startPos; 
            
            
            _box.sizeDelta = new Vector2(Mathf.Abs(fromStartToEnd.x), Mathf.Abs(fromStartToEnd.y));
            
            var middle = new Vector2( fromStartToEnd.x / 2, fromStartToEnd.y / 2);

            var setPosition = startPos + middle;
            
            _box.anchoredPosition = setPosition;


            startPos = Camera.main.ScreenToWorldPoint(startPos);
            endPos = Camera.main.ScreenToWorldPoint(endPos);
            
            fromStartToEnd.x = endPos.x - startPos.x;
            fromStartToEnd.y = endPos.y - startPos.y;
            
            middle = new Vector2( fromStartToEnd.x / 2, fromStartToEnd.y / 2);
            setPosition = startPos + middle;
             
            _boxSprite.transform.position = setPosition;
            _boxSprite.transform.localScale = fromStartToEnd;
        }


        private void OnEnable()
        {
            _zoneButton.onClick.AddListener(CreateZone);
            _miningButton.onClick.AddListener(FindMining);
            _itemButton.onClick.AddListener(FindItems);
        }

        private void CreateZone()
        {
            var entity = _mainAspect.World().NewEntity();
            _mainAspect.ZoneAspect.ZoneMode.Add(entity);
        }

        private void FindItems()
        {
            foreach (var entity in _mainAspect.SelectionAspect.SelectedItemsIt)
            {
                _mainAspect.MarkerWork.GetOrAdd(entity, out _);
            }
        }
        private void FindMining()
        {
            foreach (var entity in _mainAspect.SelectionAspect.SelectedMiningIt)
            {
                _mainAspect.MarkerWork.GetOrAdd(entity, out _);
            }
        }
    }
}