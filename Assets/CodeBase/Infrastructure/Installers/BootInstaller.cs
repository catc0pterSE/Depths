using CodeBase.Infrastructure.Providers.SceneReference;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Installers
{
    public class BootInstaller : MonoInstaller
    {
        [SerializeField] private SOSceneReferenceProvider _sceneReferenceProvider;
        
        public override void InstallBindings()
        {
            BindSceneReferenceProvider();
        }

        private void BindSceneReferenceProvider()
        {
            Container.Bind<ISceneReferenceProvider>().FromInstance(_sceneReferenceProvider).AsSingle().NonLazy();
        }

        private void BindGameStatesFactory()
        {
            
        }
    }
}