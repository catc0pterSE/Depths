using CodeBase.Common.StateMachine;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.StateMachine.States;
using Zenject;

namespace CodeBase.Common.Factories.GameStateMachineFactories
{
    public class GameStateMachineBuilder : IFactory<IGameStateMachine>
    {
        private readonly IStatesFactory _statesFactory;

        public GameStateMachineBuilder(IStatesFactory statesFactory)
        {
            _statesFactory = statesFactory;
        }

        public IGameStateMachine Build()
        {
            GameStateMachine gameStateMachine = new GameStateMachine();

            gameStateMachine.RegisterStates
            (
                new[]
                {
                    _statesFactory.Create<BootstrapState>(gameStateMachine)
                }
            );
            return gameStateMachine;
        }
    }

    public interface IGameStateMachineFactory : IFactory<IGameStateMachine>
    {
    }
}