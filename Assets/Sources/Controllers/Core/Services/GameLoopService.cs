using System;
using Sources.Controllers.Api.Presenters;
using Sources.Controllers.Api.Services;
using UnityEngine;

namespace Sources.Controllers.Core.Services
{
    public class GameLoopService : IGameLoopService
    {
        private readonly IShipPresenterFactory _shipPresenterFactory;
        private readonly IEnemySpawner _enemySpawner;
        private readonly ILevelProgressCounter _levelProgressCounter;

        private readonly Vector3 _playerSpawnPosition;
        private readonly IPersistentDataService _persistentDataService;

        private Coroutine _spawnRoutine;
        private IShipPresenter _playerShip;
        private float _bestScore;
        private bool _inProgress;

        public GameLoopService
        (
            IShipPresenterFactory shipPresenterFactory,
            IEnemySpawner enemySpawner,
            ILevelProgressCounter levelProgressCounter,
            Vector3 playerSpawnPosition,
            IPersistentDataService persistentDataService
        )
        {
            _shipPresenterFactory = shipPresenterFactory;
            _enemySpawner = enemySpawner;
            _levelProgressCounter = levelProgressCounter;
            _playerSpawnPosition = playerSpawnPosition;
            _persistentDataService = persistentDataService;
        }

        public event Action PlayerDied;
        public event Action<float> ScoreUpdated;
        public event Action<float> BestScoreChanged;

        public void Start()
        {
            _inProgress = true;
            _playerShip = _shipPresenterFactory.CreatePlayerShipPresenter();
            _playerShip.Transform.SetPositionAndRotation(_playerSpawnPosition, Quaternion.identity);
            _bestScore = _persistentDataService.GetBestScore();

            UpdateScore(_levelProgressCounter.Value);
            UpdateBestScore(_bestScore);

            _playerShip.Destroyed += OnPlayerShipDestroyed;
            _levelProgressCounter.Updated += UpdateScore;
            _persistentDataService.BestScoreChanged += UpdateBestScore;

            _levelProgressCounter.Start();
            _enemySpawner.Enable();
        }

        public void Stop()
        {
            _inProgress = false;
           
            _playerShip.Destroyed -= OnPlayerShipDestroyed;
            _levelProgressCounter.Updated -= UpdateScore;
            _persistentDataService.BestScoreChanged -= UpdateBestScore;
            
            _enemySpawner.Disable();
            _levelProgressCounter.Stop();
        }

        public void Update()
        {
            if (_inProgress == false)
                return;

            _enemySpawner.Update();
            _levelProgressCounter.Update(Time.deltaTime);
        }

        private void OnPlayerShipDestroyed(IShipPresenter _)
        {
            PlayerDied?.Invoke();
            Stop();
        }

        private void UpdateScore(float progress)
        {
            ScoreUpdated?.Invoke(progress);

            if (_bestScore < progress)
                _persistentDataService.SetBestScore(progress);
        }

        private void UpdateBestScore(float score) => 
            BestScoreChanged?.Invoke(score);
    }
}