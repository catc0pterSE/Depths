using System.Collections.Generic;
using CodeBase.Common.StateMachine;
using CodeBase.Infrastructure.StateMachine.States;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.StateMachine
{
    public class GameStateMachine : Common.StateMachine.StateMachine, IGameStateMachine
    {
        public UniTask EnterBootStrapState() =>
            Enter<BootstrapState>();

        public UniTask EnterMaiMenuState() =>
            Enter<MenuState>();
    }
}