using CodeBase.Common.StateMachine;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class MenuState: IParameterlessState
    {
        private BaseStateMachine _gameStateMachine;

        public MenuState()
        {
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