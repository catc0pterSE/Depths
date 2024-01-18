using UnityEngine;

namespace Factory.Asset
{
    public interface IAssetProvider
    {
        T Instantiate<T>(string path) where T : MonoBehaviour;
    }
}