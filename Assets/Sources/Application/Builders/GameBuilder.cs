using Sources.Infrastructure.Core;

namespace Sources.Application
{
    internal class GameBuilder
    {
        public Game Build()
        {
            SceneLoader sceneLoader = new SceneLoaderFactory().Create();

            return new Game(new GameStateMachine(sceneLoader));
        }
    }
}