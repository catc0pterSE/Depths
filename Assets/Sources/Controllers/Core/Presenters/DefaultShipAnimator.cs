using System;
using Sources.Controllers.Api.Presenters;
using Sources.Controllers.Core.Strategies;
using Sources.Domain.Models.Ships;
using UnityEngine;

namespace Sources.Controllers.Core.Presenters
{
    public class DefaultShipAnimator : MonoBehaviour, IShipAnimator
    {
        [SerializeField] private SpriteRenderer _hullRenderer;
        [SerializeField] private SpriteRenderer _exhaustRenderer;
        [SerializeField] private SpriteAnimationStrategy _explosionAnimation;
        [SerializeField] private SpriteAnimationStrategy _exhaustAnimation;

        private IShip _ship;
        private Coroutine _exhaustRoutine;
        private Coroutine _explosionRoutine;
        private bool _isInitialized;

        public event Action ExplosionCompleted;

        public void Initialize(IShip ship)
        {
            _ship = ship;
            _exhaustAnimation.Initialize(_exhaustRenderer);
            _explosionAnimation.Initialize(_hullRenderer);

            _ship.Destroyed += OnHullDestroyed;
            _explosionAnimation.Completed += OnExplosionAnimationCompleted;

            _isInitialized = true;
        }

        private void OnDestroy()
        {
            if (_isInitialized == false)
                return;

            _ship.Destroyed -= OnHullDestroyed;
            _explosionAnimation.Completed -= OnExplosionAnimationCompleted;
        }

        public void PlayExhaust()
        {
            if (_exhaustRoutine != null)
                StopCoroutine(_exhaustRoutine);

            _exhaustRenderer.enabled = true;
            _exhaustRoutine = StartCoroutine(_exhaustAnimation.Start());
        }

        public void StopExhaust()
        {
            if (_exhaustRoutine != null)
                StopCoroutine(_exhaustRoutine);

            _exhaustRenderer.enabled = false;
        }

        public void PlayExplosion()
        {
            if (_explosionRoutine != null)
                StopCoroutine(_explosionRoutine);

            _explosionRoutine = StartCoroutine(_explosionAnimation.Start());
        }

        public void Reset()
        {
            _explosionAnimation.Reset();
            _exhaustAnimation.Reset();
        }

        private void OnExplosionAnimationCompleted() => 
            ExplosionCompleted?.Invoke();

        private void OnHullDestroyed()
        {
            StopExhaust();
            PlayExplosion();
        }
    }
}