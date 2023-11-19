using Sources.Infrastructure.Api.Services;
using Sources.Infrastructure.Core;
using UnityEngine;

namespace Sources.Application
{
    internal class SceneLoaderFactory
    {
        private GameObject _gameObject;

        public SceneLoader Create()
        {
            _gameObject = new GameObject(nameof(SceneLoader));
            
            ICoroutineRunner coroutineRunner = _gameObject.AddComponent<CoroutineRunner>();
            
            return new SceneLoader(coroutineRunner);
        }
    }
}