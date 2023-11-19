using System;
using Sources.Controllers.Api.Presenters;
using Sources.Controllers.Api.Presenters.Strategies;
using Sources.Controllers.Core.Strategies;
using Sources.Domain.Models.Ships;
using UnityEngine;

namespace Sources.Controllers.Core.Presenters
{
    public class DefaultShipCollisionHandler : MonoBehaviour, IShipCollisionHandler
    {
        [SerializeField] private Collider2D _collider;
        [SerializeField] private ContactFilter2D _contactFilter;

        private ICollisionStrategy _collisionStrategy;
        private IShip _ship;
        private bool _isInitialized;
        private bool _isEnabled;

        public event Action<IShipPresenter> Impacted;

        public void Initialize(IShip ship)
        {
            _ship = ship;
            _collisionStrategy = new DefaultCollisionStrategy(_collider, _contactFilter);

            _ship.Destroyed += Disable;
            _collisionStrategy.Collided += OnCollided;
            _collisionStrategy.Enable();
            _isInitialized = true;
        }

        private void Update()
        {
            if (_isInitialized == false || _isEnabled == false)
                return;

            _collisionStrategy.Resolve();
        }

        private void OnDestroy()
        {
            if (_isInitialized == false)
                return;

            _ship.Destroyed -= Disable;
            _collisionStrategy.Collided -= OnCollided;
        }

        public void Enable()
        {
            _isEnabled = true;
            _collisionStrategy.Enable();
        }

        public void Disable()
        {
            _isEnabled = false;
            _collisionStrategy.Disable();
        }

        private void OnCollided(Collider2D impactedCollider)
        {
            if (impactedCollider.gameObject.TryGetComponent(out IShipPresenter shipPresenter))
                Impacted?.Invoke(shipPresenter);
        }
    }
}