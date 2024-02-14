using CodeBase.Infrastructure.SceneManagement.Providers.SceneReference;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.StateMachine.Factories;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Boot.Installers
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
            Container.BindInterfacesTo<SOSceneReferenceProvider>().FromInstance(_sceneReferenceProvider).AsSingle().NonLazy();

        private void BindGameStatesFactory() =>
            Container.BindInterfacesTo<GameStatesFactory>().AsSingle().NonLazy();
        
        private void BindGameStateMachine() =>
            Container.Bind<IGameStateMachine>().FromFactory<GameStateMachineBuilder>().AsSingle().NonLazy();
    }
}