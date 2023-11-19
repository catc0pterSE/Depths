using System;
using System.Collections.Generic;
using Sources.Common.WindowFsm;
using Sources.Common.WindowFsm.Windows;
using Sources.Controllers.Api.Presenters;
using Sources.Controllers.Api.Services;
using Sources.Controllers.Core.Factories;
using Sources.Controllers.Core.Services;
using Sources.Controllers.Core.ViewModels;
using Sources.Controllers.Core.WindowFsms;
using Sources.Controllers.Core.WindowFsms.Windows;
using Sources.Domain.Factories;
using Sources.Infrastructure.Api.GameFsm;
using Sources.Infrastructure.Api.Services;
using Sources.Infrastructure.Api.Services.Providers;
using Sources.Infrastructure.Core.Services.DI;
using Sources.Presentation.Core;
using UnityEngine;

namespace Sources.Application.CompositionRoots
{
    public sealed class GameLoopCompositionRoot : SceneCompositionRoot, ICoroutineRunner
    {
        [SerializeField] private GameLoopView _gameLoopView;
        [SerializeField] private LoseView _loseView;

        private IGameLoopService _gameLoopService;
        private bool _isInitialized;

        public override void Initialize(ServiceContainer serviceContainer)
        {
            IConfigurationProvider configurationProvider = serviceContainer.Single<IConfigurationProvider>();

            IShipFactory shipFactory = new ShipFactory();
            IShipPresenterFactory shipPresenterFactory = new ShipPresenterFactory(shipFactory, configurationProvider);
            IEnemySpawner enemyEnemySpawner = new EnemySpawner(shipFactory, shipPresenterFactory, this, configurationProvider);
            ILevelProgressCounter levelProgressCounter = new LevelProgressCounter();
            IPersistentDataService persistentDataService = serviceContainer.Single<IPersistentDataService>();

            _gameLoopService = new GameLoopService
            (
                shipPresenterFactory,
                enemyEnemySpawner,
                levelProgressCounter,
                configurationProvider.PlayerSpawnPosition,
                persistentDataService
            );

            Dictionary<Type, IWindow> windows = new Dictionary<Type, IWindow>()
            {
                [typeof(RootWindow)] = new RootWindow(),
                [typeof(GameLoopWindow)] = new GameLoopWindow(),
                [typeof(LoseWindow)] = new LoseWindow(),
            };

            IWindowFsm windowFsm = new WindowFsm<RootWindow>(windows);

            IGameStateMachine gameStateMachine = serviceContainer.Single<IGameStateMachine>();
            GameLoopViewModel gameLoopViewModel = new GameLoopViewModel(windowFsm, gameStateMachine, _gameLoopService);
            
            _gameLoopView.Initialize(gameLoopViewModel);
            _loseView.Initialize(gameLoopViewModel);

            _gameLoopService.Start();
            _isInitialized = true;
        }

        public void Update()
        {
            if (_isInitialized == false)
                return;

            _gameLoopService.Update();
        }
    }
}