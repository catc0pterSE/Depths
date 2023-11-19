using Sources.Controllers.Api.Presenters;
using Sources.Controllers.Core.Presenters;
using Sources.Domain.Factories;
using Sources.Domain.Models.Ships;
using Sources.Infrastructure.Api.Services.Providers;
using UnityEngine;

namespace Sources.Controllers.Core.Factories
{
    public class ShipPresenterFactory : IShipPresenterFactory
    {
        private readonly IShipFactory _shipFactory;
        private readonly IConfigurationProvider _configurationProvider;

        public ShipPresenterFactory(IShipFactory shipFactory, IConfigurationProvider configurationProvider)
        {
            _shipFactory = shipFactory;
            _configurationProvider = configurationProvider;
        }

        public IShipPresenter CreatePlayerShipPresenter()
        {
            IShipPresenter shipPresenter = Object
                .Instantiate(_configurationProvider.PlayerShipPrefab)
                .GetComponent<ShipPresenter>();
            
            IShip ship = _shipFactory.CreateDefault();

            shipPresenter.Initialize(ship);

            return shipPresenter;
        }

        public IShipPresenter CreateEnemyShipPresenter()
        {
            IShipPresenter shipPresenter = Object
                .Instantiate(_configurationProvider.EnemyShipPrefab)
                .GetComponent<ShipPresenter>();
            IShip ship = _shipFactory.CreateWeak();

            shipPresenter.Initialize(ship);

            return shipPresenter;
        }
    }
}