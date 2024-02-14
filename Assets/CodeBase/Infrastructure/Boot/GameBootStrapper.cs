using CodeBase.Infrastructure.StateMachine;
using CodeBase.UI.Loading;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Boot
{
    public class GameBootStrapper : MonoBehaviour
    {
        [SerializeField] private LoadingCurtain _loadingCurtain;
        
        private IGameStateMachine _gameStateMachine;

        [Inject]
        public GameBootStrapper Construct
        (
            IGameStateMachine gameStateMachine
        )
        {
            _gameStateMachine = gameStateMachine;
            return this;
        }

        private void Awake()
        {
            _gameStateMachine.EnterBootStrapState();
        }
    }
}