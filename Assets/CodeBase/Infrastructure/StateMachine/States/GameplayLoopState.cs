using CodeBase.Common.StateMachine;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class GameplayLoopState : IParameterlessState
    {
        private readonly GameStateMachine _gameStateMachine;

        public GameplayLoopState(GameStateMachine gameStateMachine)
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