using Sources.Controllers.Api.Presenters.Strategies;
using UnityEngine;

namespace Sources.Controllers.Core.Strategies
{
    public class PlayerMoveStrategy : IMoveStrategy
    {
        private const string MovableAxis = "Horizontal";
        
        private readonly Vector2 _horizontalConstrain = GameConstants.PlayerMoveHorizontalConstrain;
        private readonly float _moveSpeed;
        private readonly Transform _transform;

        private bool _isEnabled;

        public PlayerMoveStrategy(float moveSpeed, Transform transform)
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

            Vector3 moveDirection = new Vector3(Input.GetAxisRaw(MovableAxis), 0, 0);
            moveDirection.Normalize();

            Vector3 newPosition = _transform.position + moveDirection * (_moveSpeed * Time.deltaTime);

            if (newPosition.x > _horizontalConstrain.y || newPosition.x < _horizontalConstrain.x)
                return;

            _transform.position = newPosition;
        }
    }
}