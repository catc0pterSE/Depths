using UnityEngine;

namespace Factory.Asset
{
    public class AssetProvider : IAssetProvider
    {
        public T Instantiate<T>(string path) where T : MonoBehaviour
        {
            var prefab = Resources.Load<T>(path);
            return Object.Instantiate(prefab);
        }
    }
}