using System;
using CodeBase.Infrastructure.SceneManagement.Providers.SceneReference;
using CodeBase.UI.Loading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.SceneManagement
{
    public class AddressablesSceneLoader : ISceneLoader
    {
        private readonly LoadingCurtain _loadingCurtain;
        private readonly ISceneReferenceProvider _sceneReferenceProvider;

        public AddressablesSceneLoader(LoadingCurtain loadingCurtain, ISceneReferenceProvider sceneReferenceProvider)
        {
            _loadingCurtain = loadingCurtain;
            _sceneReferenceProvider = sceneReferenceProvider;
        }
        
        public async void LoadScene(ScenesEnum scene, Action onLoaded = null)
        {
            await _loadingCurtain.FadeInAsync();
            await Addressables.LoadSceneAsync(_sceneReferenceProvider.GetReference(scene));
            onLoaded?.Invoke();
            await _loadingCurtain.FadeOutAsync();
        }
    }
}