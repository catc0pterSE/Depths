using Factory.Asset;
using Grid.Elements;
using Grid.Elements.Work.Cell;
using UnityEngine;

namespace Factory
{
    public class Factory<T> : IFactory<T> where T : MonoBehaviour
    {
        private readonly IAssetProvider _assetProvider;
        private IFactory<T> _factoryImplementation;

        public Factory(IAssetProvider assetProvider) =>
            _assetProvider = assetProvider;

        public T Create(Transform container)
        {
            return typeof(T) switch
            {
                var t when t == typeof(CellView) => CreateElement(PathAssets.PrefabCube, container),
            };
        }

        private T CreateElement(string path, Transform container)
        {
            var prefab = _assetProvider.Instantiate<T>(path);
            prefab.transform.SetParent(container);
            return prefab;
        }
    }
}