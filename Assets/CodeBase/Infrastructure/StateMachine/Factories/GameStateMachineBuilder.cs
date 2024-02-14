using CodeBase.Infrastructure.StateMachine.States;
using Zenject;

namespace CodeBase.Infrastructure.StateMachine.Factories
{
    public class GameStateMachineBuilder : IFactory<IGameStateMachine>
    {
        private readonly IGameStatesFactory _gameStatesFactory;

        public GameStateMachineBuilder(IGameStatesFactory gameStatesFactory)
        {
            _gameStatesFactory = gameStatesFactory;
        }
        
        public IGameStateMachine Build()
        {
            GameStateMachine gameStateMachine = new GameStateMachine();

            gameStateMachine.RegisterStates
            (
                new[]
                {
                    _gameStatesFactory.Create<BootstrapState>(gameStateMachine)
                }
            );
            
            return gameStateMachine;
        }
    }
}