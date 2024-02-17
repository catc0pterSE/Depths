using CodeBase.Infrastructure.SceneManagement.Providers.SceneReference;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.StateMachine.Factories;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Boot
{
    public class BootInstaller : MonoInstaller
    {
        [SerializeField] private SOSceneReferenceProvider _sceneReferenceProvider;

        public override void InstallBindings()
        {
            BindSceneReferenceProvider();
            BindGameStatesFactory();
            BindGameStateMachine();
        }

        private void BindSceneReferenceProvider() =>
            Container.Bind<ISceneReferenceProvider>().FromInstance(_sceneReferenceProvider).AsSingle().NonLazy();

        private void BindGameStatesFactory() =>
            Container.Bind<IGameStatesFactory>().To<GameStatesFactory>().AsSingle().NonLazy();
        
        private void BindGameStateMachine() =>
            Container.Bind<IGameStateMachine>().FromFactory<GameStateMachineBuilder>().AsSingle().NonLazy();
    }
}