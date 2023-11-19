using Sources.Infrastructure.Api.GameFsm;

namespace Sources.Application
{
    internal class Game
    {
        private readonly IGameStateMachine _gameStateMachine;
    
        public Game(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }
    
        public void Run() => 
            _gameStateMachine.Enter<BootstrapState>();

        public void Update() => 
            _gameStateMachine.Update();

        public void Finish()
        {
        }
    }
}