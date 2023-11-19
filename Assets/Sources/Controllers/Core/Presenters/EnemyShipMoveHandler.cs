using Sources.Controllers.Api.Presenters;
using Sources.Controllers.Api.Presenters.Strategies;
using Sources.Controllers.Core.Strategies;
using Sources.Domain.Models.Ships;
using UnityEngine;

namespace Sources.Controllers.Core.Presenters
{
    public class EnemyShipMoveHandler : MonoBehaviour, IShipMoveHandler
    {
        private IMoveStrategy _moveStrategy;
        private IShip _ship;

        private bool _isInitialized;
        private bool _isEnabled;

        public void Initialize(IShip ship)
        {
            _ship = ship;
            _moveStrategy = new EnemyMoveStrategy(_ship.Speed, transform);

            _isInitialized = true;
        }

        private void Update()
        {
            if (_isInitialized == false || _isEnabled == false)
                return;

            _moveStrategy.Resolve();
        }

        public void Enable()
        {
            _isEnabled = true;
            _moveStrategy.Enable();
        }

        public void Disable()
        {
            _isEnabled = false;
            _moveStrategy.Disable();
        }
    }
}