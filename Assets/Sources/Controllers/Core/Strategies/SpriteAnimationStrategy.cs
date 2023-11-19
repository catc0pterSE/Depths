using System;
using System.Collections;
using UnityEngine;

namespace Sources.Controllers.Core.Strategies
{
    [Serializable]
    public class SpriteAnimationStrategy
    {
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private float _delay;
        [SerializeField] private bool _isLooped;
        [SerializeField] private bool _hideAfterComplete;

        private int _currentSpriteId;
        private bool _isEnabled;
        private SpriteRenderer _renderer;

        public event Action Completed;

        public void Initialize(SpriteRenderer spriteRenderer) => 
            _renderer = spriteRenderer;

        public IEnumerator Start()
        {
            Reset();

            WaitForSeconds delay = new WaitForSeconds(_delay);

            while (_isEnabled)
            {
                SetNextSprite();
                yield return delay;
            }

            _renderer.enabled = _hideAfterComplete == false;

            Completed?.Invoke();
        }

        public void Reset()
        {
            _currentSpriteId = 0;
            _isEnabled = true;
            _renderer.sprite = _sprites[_currentSpriteId];
            _renderer.enabled = true;
        }

        private void SetNextSprite()
        {
            _renderer.sprite = _sprites[_currentSpriteId];

            if (++_currentSpriteId == _sprites.Length)
            {
                _currentSpriteId = 0;
                _isEnabled = _isLooped;
            }
        }
    }
}