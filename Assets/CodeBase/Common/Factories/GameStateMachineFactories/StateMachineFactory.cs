using System;
using CodeBase.Common.StateMachine;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.StateMachine.States;
using Zenject;

namespace CodeBase.Common.Factories.GameStateMachineFactories
{
    public class GameStateMachineFactory : IFactory<IGameStateMachine>
    {
        private readonly IStatesFactory _statesFactory;

        public GameStateMachineFactory(IStatesFactory statesFactory)
        {
            _statesFactory = statesFactory;
        }

        public IGameStateMachine Create() =>
            new GameStateMachine
            (
                new IState[]
                {
                    _statesFactory.Create<BootstrapState>() 
                }
            );
    }

    public interface IGameStateMachineFactory : IFactory<IGameStateMachine>
    {
        
    }
}