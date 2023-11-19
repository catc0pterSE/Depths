using System.Collections;
using UnityEngine;

namespace Sources.Controllers.Core.Presenters
{
    public class SimpleSpriteAnimation : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private float _delay;

        private int _currentSpriteId;
        private bool _isEnabled;

        private IEnumerator Start()
        {
            var delay = new WaitForSeconds(_delay);
            _isEnabled = true;
            while (_isEnabled)
            {
                SetNextSprite();
                yield return delay;
            }
        }

        private void SetNextSprite()
        {
            if (++_currentSpriteId == _sprites.Length)
                _currentSpriteId = 0;
            _renderer.sprite = _sprites[_currentSpriteId];
        }

        private void OnDestroy()
        {
            _isEnabled = false;
        }
    }
}