using System;
using Sources.Controllers.Api.Presenters.Strategies;
using UnityEngine;

namespace Sources.Controllers.Core.Strategies
{
    public class DefaultCollisionStrategy : ICollisionStrategy
    {
        private readonly Collider2D _collider;
        private readonly ContactFilter2D _contactFilter;
        private readonly Collider2D[] _collisionResult;

        private bool _isEnabled;

        public DefaultCollisionStrategy(Collider2D collider, ContactFilter2D contactFilter)
        {
            _collider = collider;
            _contactFilter = contactFilter;
            _collisionResult = new Collider2D[GameConstants.MaxCollisionPerShip];
        }

        public event Action<Collider2D> Collided;

        public void Enable()
        {
            _collider.enabled = true;
            _isEnabled = true;
        }

        public void Disable()
        {
            _collider.enabled = false;
            _isEnabled = false;
        }

        public void Resolve()
        {
            if (_isEnabled == false)
                return;

            int collisionsCount = _collider.OverlapCollider(_contactFilter, _collisionResult);

            for (int i = 0; i < collisionsCount; i++) 
                Collided?.Invoke(_collisionResult[i]);
        }
    }
}