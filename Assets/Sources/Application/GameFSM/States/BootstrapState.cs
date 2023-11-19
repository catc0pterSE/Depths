using Sources.Controllers.Api.Services;
using Sources.Controllers.Core.Services;
using Sources.Infrastructure.Api.GameFsm;
using Sources.Infrastructure.Api.Services;
using Sources.Infrastructure.Api.Services.Providers;
using Sources.Infrastructure.Core;
using Sources.Infrastructure.Core.Services;
using Sources.Infrastructure.Core.Services.DI;
using Sources.Infrastructure.Core.Services.Providers;
using UnityEngine;

namespace Sources.Application
{
    public class BootstrapState : IState
    {
        private const string BootstrapScene = "Bootstrap";

        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly ServiceContainer _services;

        public BootstrapState
        (
            GameStateMachine gameStateMachine,
            SceneLoader sceneLoader,
            ServiceContainer serviceContainer
        )
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _services = serviceContainer;
        }

        public void Enter()
        {
            RegisterServices();

            _sceneLoader.Load(BootstrapScene, onLoaded: EnterLoadLevel);
        }

        public void Exit()
        {
        }

        public void Update()
        {
        }

        private void RegisterServices()
        {
            ConfigurationContainer configurationContainer = new ResourceProvider().Load<ConfigurationContainer>();
            IConfigurationProvider configurationProvider = new ConfigurationProvider(configurationContainer);
            ICoroutineRunner coroutineRunner = new GameObject(nameof(CoroutineRunner)).AddComponent<CoroutineRunner>();
            ISaveService saveService = new SaveService();
            IPersistentDataService persistentDataService = new PersistentDataService(saveService);

            _services.RegisterAsSingle<IGameStateMachine>(_gameStateMachine);
            _services.RegisterAsSingle(coroutineRunner);
            _services.RegisterAsSingle(configurationProvider);
            _services.RegisterAsSingle(persistentDataService);

            _services.LockRegister();
        }

        private void EnterLoadLevel() => 
            _gameStateMachine.Enter<MainMenuState>();
    }
}