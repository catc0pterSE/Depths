using UnityEngine;

namespace DefaultNamespace 
{
    public class Factory<T> : IFactory<T> where T : MonoBehaviour
    {
        public T Create(T prefab, Transform container) => 
            GameObject.Instantiate(prefab, container);
    }
}