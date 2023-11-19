using Sources.Controllers.Api.Presenters.Strategies;
using UnityEngine;

namespace Sources.Controllers.Core.Strategies
{
    public class EnemyMoveStrategy : IMoveStrategy
    {
        private readonly float _moveSpeed;
        private readonly Transform _transform;

        private bool _isEnabled;

        public EnemyMoveStrategy(float moveSpeed, Transform transform)
        {
            _moveSpeed = moveSpeed;
            _transform = transform;
        }

        public void Enable() => 
            _isEnabled = true;

        public void Disable() => 
            _isEnabled = false;

        public void Resolve()
        {
            if (_isEnabled == false)
                return;
            
            _transform.Translate(-_transform.up * (_moveSpeed * Time.deltaTime));
        }
    }
}