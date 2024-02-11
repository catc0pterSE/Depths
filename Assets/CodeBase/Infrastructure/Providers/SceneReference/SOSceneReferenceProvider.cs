using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.SceneManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Infrastructure.Providers.SceneReference
{
    [CreateAssetMenu(menuName = "Create ScenesSO", fileName = "ScenesSO", order = 51)]
    public class SOSceneReferenceProvider : ScriptableObject, ISceneReferenceProvider
    {
        [SerializeField] private SceneReference[] _sceneReferences;
        
        private Dictionary<ScenesEnum, AssetReference> _dictionary;
        private Dictionary<ScenesEnum, AssetReference> Dictionary =>
            _dictionary ??= _sceneReferences.ToDictionary(data => data.Scene, data => data.Reference);
        
        public string GetReference(ScenesEnum scenesEnum) =>
            Dictionary[scenesEnum].AssetGUID;
        
        [Serializable]
        private class SceneReference
        {
            public ScenesEnum Scene;
            public AssetReference Reference;
        }
    }
}