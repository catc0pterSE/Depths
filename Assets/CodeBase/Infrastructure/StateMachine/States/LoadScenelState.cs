using System;
using CodeBase.Common.StateMachine;
using CodeBase.Infrastructure.SceneManagement;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class LoadScenelState: IParameterState<string, Action>
    {
        private readonly GameStateMachine _gameStateMachine;
        private ISceneLoader _sceneLoader;

        public LoadScenelState(ISceneLoader sceneLoader, GameStateMachine gameStateMachine)
        {
            _sceneLoader = sceneLoader;
            _gameStateMachine = gameStateMachine;
        }

        public UniTask Enter(string sceneName, Action enterNextScene)
        {
            OnSceneLoaded();
            return default;
        }

        public UniTask Exit()
        {
            return default;
        }

        private void OnSceneLoaded()
        {
           
        }
    }
}