using Sources.Infrastructure.Api.GameFsm;
using Sources.Infrastructure.Core;
using Sources.Infrastructure.Core.Services.DI;

namespace Sources.Application
{
    public class GameLoopState : IState
    {
        private const string GameLoopScene = "GameLoopScene";

        private readonly SceneLoader _sceneLoader;
        private readonly ServiceContainer _serviceContainer;

        public GameLoopState(SceneLoader sceneLoader, ServiceContainer serviceContainer)
        {
            _sceneLoader = sceneLoader;
            _serviceContainer = serviceContainer;
        }

        public void Enter() =>
            _sceneLoader.Load(GameLoopScene, OnSceneLoaded);

        public void Exit()
        {
        }

        public void Update()
        {
        }

        private void OnSceneLoaded() => 
            new SceneInitializer().Initialize(_serviceContainer);
    }
}