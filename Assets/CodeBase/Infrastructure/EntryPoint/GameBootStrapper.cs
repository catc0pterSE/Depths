using CodeBase.Common.Factories.GameStateMachineFactories;
using CodeBase.Infrastructure.Providers.SceneReference;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.UI.Loading;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.EntryPoint
{
    public class GameBootStrapper : MonoBehaviour
    {
        [SerializeField] private LoadingCurtain _loadingCurtain;
        
        private IGameStateMachineFactory _gameStateMachineFactory;

        [Inject]
        public GameBootStrapper Construct
        (
            IGameStateMachineFactory gameStateMachine,
            ISceneReferenceProvider sceneReferenceProvider
        )
        {
            _gameStateMachineFactory = gameStateMachine;
            return this;
        }

        private void Awake()
        {
            
        }
    }
}