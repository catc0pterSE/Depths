using System.Collections;
using System.Collections.Generic;
using Sources.Controllers.Api.Presenters;
using Sources.Controllers.Api.Services;
using Sources.Domain.Factories;
using Sources.Infrastructure.Api.Services;
using Sources.Infrastructure.Api.Services.Providers;
using UnityEngine;
using UnityEngine.Pool;
using Random = System.Random;

namespace Sources.Controllers.Core.Services
{
    public class EnemySpawner : IEnemySpawner
    {
        private readonly IShipFactory _shipFactory;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IObjectPool<IShipPresenter> _shipsPool;
        
        private readonly Vector3 _playerSpawnPosition;
        private readonly Vector3 _middleSpawnPoint;
        private readonly Quaternion _enemySpawnRotation;
        private readonly int _maxDistanceFromSpawn;
        private readonly WaitForSeconds _spawnDelay;
        private readonly List<Vector3> _availableSpawnPoints;
        private readonly List<IShipPresenter> _enemyShips;
        private readonly Random _random;
        
        private bool _isGameInProgress;
        private Coroutine _spawnRoutine;

        public EnemySpawner
        (
            IShipFactory shipFactory,
            IShipPresenterFactory shipPresenterFactory,
            ICoroutineRunner coroutineRunner,
            IConfigurationProvider configurationProvider)
        {
            _shipFactory = shipFactory;
            _coroutineRunner = coroutineRunner;
            _configurationProvider = configurationProvider;
            _shipsPool = new ObjectPool<IShipPresenter>(shipPresenterFactory.CreateEnemyShipPresenter);

            _middleSpawnPoint = _configurationProvider.MiddleSpawnPoint;
            _enemySpawnRotation = Quaternion.Euler(GameConstants.EnemySpawnRotation);
            _maxDistanceFromSpawn = configurationProvider.MaxDistanceFromSpawn;
            _spawnDelay = new WaitForSeconds(configurationProvider.SpawnInterval);
            _availableSpawnPoints = new List<Vector3>();
            _enemyShips = new List<IShipPresenter>();
            _random = new Random();
        }

        public void Enable()
        {
            _isGameInProgress = true;

            CalculateAvailableSpawnPoints();

            if (_spawnRoutine != null)
                _coroutineRunner.StopCoroutine(_spawnRoutine);

            _spawnRoutine = _coroutineRunner.StartCoroutine(SpawnEnemy());
        }

        public void Disable()
        {
            _isGameInProgress = false;
        }

        private void CalculateAvailableSpawnPoints()
        {
            float positionXDelta = _configurationProvider.HorizontalPositionStep;

            _availableSpawnPoints.Add(_middleSpawnPoint + new Vector3(-positionXDelta, 0, 0));
            _availableSpawnPoints.Add(_middleSpawnPoint + new Vector3(0, 0, 0));
            _availableSpawnPoints.Add(_middleSpawnPoint + new Vector3(positionXDelta, 0, 0));
        }

        private IEnumerator SpawnEnemy()
        {
            while (_isGameInProgress)
            {
                IShipPresenter enemy = SpawnAt(GetNextSpawnPosition(), _enemySpawnRotation);
                _enemyShips.Add(enemy);
                yield return _spawnDelay;
            }
        }

        private bool CheckReleaseDistance(IShipPresenter shipPresenter) => 
            Vector3.Distance(shipPresenter.Transform.position, _middleSpawnPoint) >
            _maxDistanceFromSpawn;

        private Vector3 GetNextSpawnPosition() => 
            _availableSpawnPoints[_random.Next(_availableSpawnPoints.Count)];

        public IShipPresenter SpawnAt(Vector3 position, Quaternion rotation)
        {
            IShipPresenter shipPresenter = _shipsPool.Get();
            shipPresenter.ReadyToRelease += Release;
            shipPresenter.Transform.SetPositionAndRotation(position, rotation);
            shipPresenter.Initialize(_shipFactory.CreateWeak());

            return shipPresenter;
        }

        private void Release(IShipPresenter shipPresenter)
        {
            shipPresenter.ReadyToRelease -= Release;
            _shipsPool.Release(shipPresenter);
        }

        public void Update()
        {
            _enemyShips.RemoveAll(ship => ship.InUse == false);

            foreach (IShipPresenter shipPresenter in _enemyShips)
                if (CheckReleaseDistance(shipPresenter))
                    shipPresenter.Release();
        }
    }
}