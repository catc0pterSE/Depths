using CodeBase.Infrastructure.StateMachine;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Boot
{
    public class GameBootstrapper : MonoBehaviour
    {
        private IGameStateMachine _gameStateMachine;

        [Inject]
        public GameBootstrapper Construct
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