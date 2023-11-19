using Sources.Application.Factories;
using Sources.Application.GameFSM;
using Sources.Infrastructure.Core;

namespace Sources.Application.Builders
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