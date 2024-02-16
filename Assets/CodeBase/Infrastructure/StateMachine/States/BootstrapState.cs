using CodeBase.Common.StateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.StateMachine.States
{
    public class BootstrapState : IParameterlessState
    {
        private readonly IGameStateMachine _stateMachine;
        
        public BootstrapState(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
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