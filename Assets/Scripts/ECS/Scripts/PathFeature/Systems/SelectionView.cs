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

        [SerializeField] private Button _miningButton;
        [SerializeField] private Button _itemButton;

        private MainAspect _mainAspect;

        private SpriteRenderer _spriteRenderer;
        public void Construct(MainAspect aspect)
        {
            _mainAspect = aspect;
        }
        public void SetScreen(Vector2 startPos, Vector2 endPos)
        {
            float width = endPos.x - startPos.x;
            float height = endPos.y - startPos.y;
            _box.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
            _box.anchoredPosition = startPos + new Vector2( width / 2, height / 2);
        }


        private void OnEnable()
        {
            _miningButton.onClick.AddListener(FindMining);
            _itemButton.onClick.AddListener(FindItems);
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