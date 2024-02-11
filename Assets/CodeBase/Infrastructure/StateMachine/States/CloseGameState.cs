using CodeBase.Common.StateMachine;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class CloseGameState : IParameterlessState
    {
        private readonly GameStateMachine _gameStateMachine;

        public CloseGameState(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public UniTask Enter()
        {
            return default;
        }

        public UniTask Exit()
        {
            return default;
        }
    }
}